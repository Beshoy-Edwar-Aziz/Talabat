using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using ShippingAddress = Talabat.Core.Entities.OrderAggregate.Address;
using Address = Talabat.Core.Entities.Identity.Address;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                    .ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                    .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                    .ForMember(d => d.PictureUrl, O => O.MapFrom<PictureUrlResolver>());
            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, ShippingAddress>().ReverseMap();
            CreateMap<CustomerBasketDTO, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemsDTO, BasketItems>();
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethodName, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryAmount, O => O.MapFrom(S => S.DeliveryMethod.Cost));
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.OrderedProductDetails.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.OrderedProductDetails.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.OrderedProductDetails.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<PictureUrlOfOrderItemResolver>());
        }
    }
}
