using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.APIs.Extensions;
using AutoMapper;
namespace Talabat.APIs.Controllers
{
    
    public class AccountController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            if (EmailExists(model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400,"Email Already Exists"));
            }
            if (model is not null) 
            {
                var User = new AppUser()
                {
                    DisplayName = model.DisplayName,
                    Email = model.Email,
                    UserName = model.Email.Split("@")[0],
                    PhoneNumber = model.PhoneNumber,
                };
                var Result =await _userManager.CreateAsync(User,model.Password);
                if (Result.Succeeded)
                {
                    var UserDto = new UserDTO()
                    {
                        DisplayName = model.DisplayName,
                        Email = model.Email,
                        Token = await _tokenService.CreateToken(User,_userManager)
                    };

                    return Ok(UserDto);
                }
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        return Ok(error);
                    }
                   return BadRequest(new ApiResponse(400));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO LoginModel)
        {
            if (LoginModel is not null)
            {
               var User = await _userManager.FindByEmailAsync(LoginModel.Email);
                if (User is null)
                {
                    return Unauthorized(new ApiResponse(401));
                }
                else
                {
                    var UserAuth = await _signInManager.CheckPasswordSignInAsync(User, LoginModel.Password, false);
                    if (UserAuth.Succeeded)
                    {
                        return Ok(new UserDTO()
                        {
                            DisplayName = User.DisplayName,
                            Email = User.Email,
                            Token = await _tokenService.CreateToken(User, _userManager)
                        });
                    }
                    else
                    {
                        return Unauthorized(new ApiResponse(401));
                    }
                }
            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }
        }
        [Authorize]
        [HttpGet("GetUser")]
        public async Task<ActionResult<UserDTO>> GetUserAsync() 
        {
            var GetEmail = User.FindFirstValue(ClaimTypes.Email);
            var Result=await _userManager.FindByEmailAsync(GetEmail);
            if (Result is not null)
            {
                var MappedUser = new UserDTO()
                {
                    Email = Result.Email,
                    DisplayName = Result.DisplayName,
                    Token = await _tokenService.CreateToken(Result,_userManager)
                };
                return Ok(MappedUser);
            }
            return BadRequest(new ApiResponse(400));
        }
        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
        {
            var UserAddress =await _userManager.GetCurrentUserWithAddress(User);
            var Address = UserAddress.Address;
            var MappedAddress=_mapper.Map<Address, AddressDTO>(Address);
            return Ok(MappedAddress);

        }
        [Authorize]
        [HttpPut("UpdateCurrentUserAddress")]
        public async Task<ActionResult<AddressDTO>> UpdateCurrentUserAddress(AddressDTO NewAddress)
        {
            var UserAddress = await _userManager.GetCurrentUserWithAddress(User);
            var MappedAdd = _mapper.Map<AddressDTO,Address>(NewAddress);
            MappedAdd.Id = UserAddress.Address.Id;
            UserAddress.Address = MappedAdd;
            var Result = await _userManager.UpdateAsync(UserAddress);
            if (Result.Succeeded)
            {
                return Ok(NewAddress);
            }
            else
            {
                return BadRequest(new ApiResponse(400,"Something Went Wrong"));
            }
        }
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> EmailExists(string email)
        {
            var Result =await _userManager.FindByEmailAsync(email);
            return Result is not null ? true : false;

        }
        
    }
}
