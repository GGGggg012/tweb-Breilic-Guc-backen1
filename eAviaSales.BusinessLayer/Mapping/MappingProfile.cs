using AutoMapper;
using eAviaSales.Domain.Entities;
using eAviaSales.Domain.Models;

namespace eAviaSales.BusinessLayer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserData, UserDto>();
            CreateMap<UserRegisterDto, UserData>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RegisteredOn, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());

            CreateMap<ProductData, ProductDto>()
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.IsAvailable()));
            CreateMap<ProductDto, ProductData>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<ProductData, TicketDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.FlightDate));

            CreateMap<OrderItemData, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItemData>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            CreateMap<OrderData, OrderDto>();
            CreateMap<OrderDto, OrderData>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OrderStatus.Pending));
        }
    }
}
