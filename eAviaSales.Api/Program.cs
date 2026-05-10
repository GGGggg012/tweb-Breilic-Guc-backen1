using eAviaSales.Api.Middleware;
using eAviaSales.BusinessLayer.Mapping;
using eAviaSales.DataAccess;
using eAviaSales.DataAccess.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(
        new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<MappingProfile>();
}, typeof(Program));

// РљРѕРЅС‚СЂРѕР»Р»РµСЂС‹ СЃРѕР·РґР°СЋС‚ BusinessLogic РІСЂСѓС‡РЅСѓСЋ (С„Р°Р±СЂРёРєР° flows).
// IMapper Рё IConfiguration РёРЅР¶РµРєС‚СЏС‚СЃСЏ С‡РµСЂРµР· РІСЃС‚СЂРѕРµРЅРЅС‹Р№ DI.

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "eAviaSales API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:4173", "http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());

    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

DbSession.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

EnsureDatabase();

app.Run();

void EnsureDatabase()
{
    using var u = new UserContext();
    using var p = new ProductContext();
    using var o = new OrderContext();

    if (u.Database.GetMigrations().Any())
    {
        u.Database.Migrate();
        p.Database.Migrate();
        o.Database.Migrate();
        EnsureProductsIsActiveColumn(p);
        return;
    }

    u.Database.EnsureCreated();

    EnsureTablesFor(p);
    EnsureTablesFor(o);
    EnsureProductsIsActiveColumn(p);
}

void EnsureProductsIsActiveColumn(ProductContext productCtx)
{
    try
    {
        productCtx.Database.ExecuteSqlRaw("""
            IF COL_LENGTH('Products', 'IsActive') IS NULL
                ALTER TABLE Products ADD IsActive bit NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT 1;
            """);
    }
    catch
    {
        /* СѓР¶Рµ РµСЃС‚СЊ РєРѕР»РѕРЅРєР° РёР»Рё С‚Р°Р±Р»РёС†Р° СЃ РґСЂСѓРіРёРј РёРјРµРЅРµРј вЂ” Р»РѕРєР°Р»СЊРЅРѕ РјРѕР¶РЅРѕ РґРѕР±Р°РІРёС‚СЊ РІСЂСѓС‡РЅСѓСЋ */
    }
}

void EnsureTablesFor(DbContext ctx)
{
    var creator = (IRelationalDatabaseCreator)ctx.GetService<IDatabaseCreator>();

    var script = creator.GenerateCreateScript();
    foreach (var stmt in script.Split(new[] { ";\r\n", ";\n", ";" }, StringSplitOptions.RemoveEmptyEntries))
    {
        var sql = stmt.Trim();
        if (string.IsNullOrEmpty(sql) || sql.Equals("GO", StringComparison.OrdinalIgnoreCase)) continue;
        try { ctx.Database.ExecuteSqlRaw(sql); } catch { }
    }
}
