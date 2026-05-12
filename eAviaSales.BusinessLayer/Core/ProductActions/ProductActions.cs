using AutoMapper;
using eAviaSales.DataAccess.Context;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace eAviaSales.BusinessLayer.Core.ProductActions
{
    public class ProductActions
    {
        private readonly IMapper _mapper;

        public ProductActions(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected List<ProductDto> GetAllProducts()
        {
            using var ctx = new ProductContext();
            return _mapper.Map<List<ProductDto>>(
                ctx.Products.Where(p => p.IsActive).OrderBy(p => p.Id).ToList());
        }

        protected ProductDto? GetProductById(int id)
        {
            using var ctx = new ProductContext();
            var p = ctx.Products.FirstOrDefault(x => x.Id == id && x.IsActive);
            return p == null ? null : _mapper.Map<ProductDto>(p);
        }

        protected ProductDto CreateProduct(ProductDto dto)
        {
            using var ctx = new ProductContext();
            var product = _mapper.Map<ProductData>(dto);
            product.IsActive = true;
            ctx.Products.Add(product);
            ctx.SaveChanges();
            return _mapper.Map<ProductDto>(product);
        }

        protected ProductDto? UpdateProduct(int id, ProductDto dto)
        {
            using var ctx = new ProductContext();
            var product = ctx.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Airline = dto.Airline;
            product.AirlineCode = dto.AirlineCode;
            product.Route = dto.Route;
            product.FlightDate = dto.FlightDate;
            product.Stops = dto.Stops;
            product.DurationMin = dto.DurationMin;
            ctx.SaveChanges();
            return _mapper.Map<ProductDto>(product);
        }

        protected bool DeleteProduct(int id)
        {
            using var ctx = new ProductContext();
            var product = ctx.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return false;
            ctx.Products.Remove(product);
            ctx.SaveChanges();
            return true;
        }

        protected ProductDto? SetProductActive(int id, bool isActive)
        {
            using var ctx = new ProductContext();
            var product = ctx.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return null;
            product.IsActive = isActive;
            ctx.SaveChanges();
            return _mapper.Map<ProductDto>(product);
        }

        protected List<ProductDto> GetAllProductsForAdmin()
        {
            using var ctx = new ProductContext();
            return _mapper.Map<List<ProductDto>>(ctx.Products.OrderBy(p => p.Id).ToList());
        }
    }
}
