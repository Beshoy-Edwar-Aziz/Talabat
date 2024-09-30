using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repositiories;
using Talabat.Core.Specifications;
using Talabat.Repository;

namespace Talabat.APIs.Controllers
{
   
    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            
        }
        [Authorize]
        [Cached(600)]
        [HttpGet]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery]ProductSpec Params)
        {
            var Spec = new ProductWithBrandAndTypeSpec(Params);
            var products =await _unitOfWork.Repository<Product>().GetEntitiesWithSpecAsync(Spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var NewSpec = new ProductWithCountSpec(Params);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(NewSpec);
            var ReturnPagination = new Pagination<ProductToReturnDto>()
            {
                PageIndex = Params.PageIndex,
                PageSize = Params.PageSize,
                Data = MappedProducts,
                Count = Count
            };
            return Ok(ReturnPagination);
        }
        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpec(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            if(product is null)
            {
                return NotFound(new ApiResponse(404));
            }
            var MappedProduct = _mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(MappedProduct);
        }
        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Type = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Type);
        }
        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brand = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brand);
        }
    }
}
