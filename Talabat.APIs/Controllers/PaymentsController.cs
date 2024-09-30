using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        const string endpointSecret = "whsec_mZJ7Yy4qdb602hG82cSUJpix6oRxSxtD";

        public PaymentsController(IPaymentService paymentService,
            IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        [HttpPost("{BasketId}")]
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasketDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDTO>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var CustomerBasket = await _paymentService.CreateOrUpdatePaymentIntentAsync(BasketId);
            if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "Something Went Wrong While Creating Payment"));
            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDTO>(CustomerBasket);
            return Ok(MappedBasket);
        }
        [HttpPost("StripeWebHook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                                   Request.Headers["Stripe-Signature"], endpointSecret);
                // Handle the event
                var PaymentIntent  = stripeEvent.Data.Object as PaymentIntent;
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await _paymentService.UpdatePaymentStatusAsync(PaymentIntent.Id,true);
                }
                else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    await _paymentService.UpdatePaymentStatusAsync(PaymentIntent.Id, false);

                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
