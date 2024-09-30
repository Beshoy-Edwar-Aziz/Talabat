using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
   
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(OrderToReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedShippingAddress = _mapper.Map<AddressDTO, Address>(orderDTO.ShippingAddress);
            var Order = await _orderService.CreateOrderAsync(BuyerEmail,orderDTO.BasketId,orderDTO.DeliveryMethodId,MappedShippingAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "Something Went Wrong While Creating Order"));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDTO>(Order);
            return Ok(MappedOrder);
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForSpecificUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificUser(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404,"Orders Were Not Found For This User"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(Orders);
            return Ok(MappedOrders);
        }
        [HttpGet("{OrderId}")]
        [Authorize]
        [ProducesResponseType(typeof(OrderToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForSpecificUser(int OrderId)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForSpecificUser(BuyerEmail, OrderId);
            if (Order is null) return NotFound(new ApiResponse(404, $"Order By Id {OrderId} Was Not Found"));
            var MappedOrder = _mapper.Map<Order, OrderToReturnDTO>(Order);
            return Ok(MappedOrder);
        }
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return Ok(DeliveryMethods);
        }
    }
}
