using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositiories;

namespace Talabat.APIs.Controllers
{
    
    public class BasketController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        
        [HttpGet("{BasketId}")]
        public async Task<ActionResult<CustomerBasket>> GetOrRecreateBasket(string BasketId)
        {
           var Result= await _basketRepository.GetBasketAsync(BasketId);
            if(Result is null)
            {
               return new CustomerBasket(BasketId);
            }
            else
            {
                return Ok(Result);
            }
        }
       
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateOrCreate(CustomerBasketDTO Basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDTO, CustomerBasket>(Basket);
            var Result = await _basketRepository.UpdateBasketAsync(MappedBasket);
            if(Result is null)
            {
                return BadRequest(new ApiResponse(400));
            }
            else
            {
                return Ok(Result);
            }
        }
        
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        => await _basketRepository.DeleteBasketAsync(BasketId);
            
        
    }
}
