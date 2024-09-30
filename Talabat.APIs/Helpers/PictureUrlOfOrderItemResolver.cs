using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.APIs.Helpers
{
    public class PictureUrlOfOrderItemResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public PictureUrlOfOrderItemResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            
            if (!string.IsNullOrEmpty(source.OrderedProductDetails.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.OrderedProductDetails.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
