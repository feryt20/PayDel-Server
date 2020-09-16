using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PayDel.Common.ErrorsAndMessages;
using PayDel.Common.Helpers;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;
using PayDel.Services.Site.Admin.Util;

namespace PayDel.Presentation.Controllers.Site.V1.Admin
{

    [Route("v1/site/admin/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthController> _logger;
        private readonly IUtilitiess _utilities;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ISmsService _smsService;
        private ApiReturn<string> errorModel;
        public AuthController(IUnitOfWork<PayDelDbContext> dbContext, IAuthService authService, 
            IMapper mapper, ILogger<AuthController> logger, IUtilitiess utilities,
            UserManager<User> userManager, SignInManager<User> signInManager, ISmsService smsService)
        {
            _db = dbContext;
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
            _utilities = utilities;
            _userManager = userManager;
            _signInManager = signInManager;
            _smsService = smsService;

            errorModel = new ApiReturn<string>
            {
                Status = false,
                Message = "",
                Result = null
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = new User
            {
                Name = userForRegisterDto.Name,
                UserName = userForRegisterDto.UserName,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                DateOfBirth = DateTime.Now,
                IsActive = true,
                Confirmed = true,
            };

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    user = userForRegisterDto
                });
            }

            return BadRequest(new ReturnMessage()
            {
                status = false,
                title = "خطا",
                message = "نام کاربری قبلا ثبت شده",
                code = "400"
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.UserName);
            if (user == null)
            {
                _logger.LogWarning($"{userForLoginDto.UserName} درخواست لاگین ناموفق داشته است");
                return Unauthorized("کاربری با این یوزر و پس وجود ندارد");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);
            if (result.Succeeded)
            {
                var appUser = _userManager.Users.Include(p => p.Photos)
                       .FirstOrDefault(u => u.NormalizedUserName == userForLoginDto.UserName.ToUpper());

                var userForReturn = _mapper.Map<UserForLoginDto>(appUser);
                _logger.LogInformation($"{userForLoginDto.UserName} لاگین کرده است");
                return Ok(new
                {
                    token = await _utilities.GenerateJwtTokenAsync(appUser, userForLoginDto.IsRemember),
                    user = userForReturn
                });
            }
            else
            {
                _logger.LogWarning($"{userForLoginDto.UserName} درخواست لاگین ناموفق داشته است");
                return Unauthorized("کاربری با این یوزر و پس وجود ندارد");
            }
        }


        //[Authorize(Roles ="Admin")]
        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("getval")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db._UserRepository.GetAllAsync(null, null, "Photos,BankCards");

            var userProfile = _mapper.Map<IEnumerable<UserProfileDto>>(users);
            return Ok(userProfile);
        }

        /// <summary>
        /// //////////////////////
        /// </summary>
        /// <param name="getVerificationCodeDto"></param>
        /// <returns></returns>
        [HttpPost("code")]
        [ProducesResponseType(typeof(ApiReturn<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiReturn<int>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVerificationCode(GetVerificationCodeDto getVerificationCodeDto)
        {
            var model = new ApiReturn<int>
            {
                Result = 0
            };
            getVerificationCodeDto.Mobile = getVerificationCodeDto.Mobile.ToMobile();
            if (getVerificationCodeDto.Mobile == null)
            {
                model.Status = false;
                model.Message = "شماره موبایل صحیح نمیباشد مثال : 09121234567";
                return BadRequest(model);
            }
            var OtpId = getVerificationCodeDto.Mobile + "-OTP";

            var verfyCodes = await _db._VerificationCodeRepository.GetAllAsync();
            foreach (var vc in verfyCodes.Where(p => p.RemoveDate < DateTime.Now))
            {
                if (vc.RemoveDate < DateTime.Now)
                {
                    _db._VerificationCodeRepository.Delete(vc.Id);
                }
                await _db.SaveAcync();
            }

            var oldOTP = verfyCodes.SingleOrDefault(p => p.Id == OtpId);
            if (oldOTP != null)
            {
                if (oldOTP.ExpirationDate > DateTime.Now)
                {
                    var seconds = Math.Abs((DateTime.Now - oldOTP.ExpirationDate).Seconds);
                    model.Status = false;
                    model.Message = "لطفا " + seconds + " ثانیه دیگر دوباره امتحان کنید ";
                    model.Result = seconds;
                    return BadRequest(model);
                }
                else
                {
                    _db._VerificationCodeRepository.Delete(OtpId);
                    await _db.SaveAcync();
                }
            }
            //
            var user = await _db._UserRepository.GetAsync(p => p.UserName == getVerificationCodeDto.Mobile);
            if (user == null)
            {
                var randomOTP = new Random().Next(10000, 99999);
                if (_smsService.SendVerificationCode(getVerificationCodeDto.Mobile, randomOTP.ToString()))
                {
                    var vc = new VerificationCode
                    {
                        Code = randomOTP.ToString(),
                        ExpirationDate = DateTime.Now.AddSeconds(60),
                        RemoveDate = DateTime.Now.AddMinutes(2)
                    };
                    vc.Id = OtpId;
                    //
                    await _db._VerificationCodeRepository.InsertAsync(vc);
                    await _db.SaveAcync();

                    model.Status = true;
                    model.Message = "کد فعال سازی با موفقیت ارسال شد";
                    model.Result = (int)(vc.ExpirationDate - DateTime.Now).TotalSeconds;
                    return Ok(model);
                }
                else
                {
                    model.Status = false;
                    model.Message = "خطا در ارسال کد فعال سازی";
                    return BadRequest(model);
                }
            }
            else
            {
                model.Status = false;
                model.Message = "کاربری با این شماره موبایل از قبل وجود دارد";
                return BadRequest(model);
            }
        }

        [HttpPost("register2")]
        [ProducesResponseType(typeof(ApiReturn<UserForDetailedDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register2(UserForRegisterDto userForRegisterDto)
        {
            var model = new ApiReturn<UserForDetailedDto>
            {
                Status = true
            };
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToMobile();
            if (userForRegisterDto.UserName == null)
            {
                model.Status = false;
                model.Message = "شماره موبایل صحیح نمیباشد مثال : 09121234567";
                return BadRequest(model);
            }
            var OtpId = userForRegisterDto.UserName + "-OTP";
            //
            var code = await _db._VerificationCodeRepository.GetByIdAsync(OtpId);
            if (code == null)
            {
                errorModel.Message = "کد فعالسازی صحیح نمباشد اقدام به ارسال دوباره ی کد بکنید";
                return BadRequest(errorModel);
            }
            if (code.ExpirationDate < DateTime.Now)
            {
                _db._VerificationCodeRepository.Delete(OtpId);
                await _db.SaveAcync();
                errorModel.Message = "کد فعالسازی منقضی شده است اقدام به ارسال دوباره ی کد بکنید";
                return BadRequest(errorModel);
            }
            if (code.Code == userForRegisterDto.Code)
            {
                var userToCreate = new User
                {
                    UserName = userForRegisterDto.UserName,
                    Name = userForRegisterDto.Name,
                    PhoneNumber = userForRegisterDto.UserName,
                    Address = "",
                    City = "",
                    Gender = true,
                    DateOfBirth = DateTime.Now,
                    IsActive = true,
                    PhoneNumberConfirmed = true
                };
                var photoToCreate = new Photo
                {
                    UserId = userToCreate.Id,
                    Url = string.Format("{0}://{1}{2}/{3}",
                        Request.Scheme,
                        Request.Host.Value ?? "",
                        Request.PathBase.Value ?? "",
                        "wwwroot/Files/Pic/profilepic.png"), //"https://res.cloudinary.com/keyone2693/image/upload/v1561717720/768px-Circle-icons-profile.svg.png",
                    Description = "Profile Pic",
                    Alt = "Profile Pic",
                    IsMain = true
                };
               

                var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

                if (result.Succeeded)
                {
                    var creaatedUser = await _userManager.FindByNameAsync(userToCreate.UserName);
                    await _userManager.AddToRolesAsync(creaatedUser, new[] { "User" });

                    var userForReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

                    _logger.LogInformation($"{userForRegisterDto.Name} - {userForRegisterDto.UserName} ثبت نام کرده است");
                    //
                    model.Message = "ثبت نام شما با موفقیت انجام شد";
                    model.Result = userForReturn;
                    return CreatedAtRoute("GetUser", new
                    {
                        controller = "Users",
                        v = HttpContext.GetRequestedApiVersion().ToString(),
                        id = userToCreate.Id
                    }, model);
                }
                else if (result.Errors.Any())
                {
                    _logger.LogWarning(result.Errors.First().Description);
                    //
                    errorModel.Message = result.Errors.First().Description;
                    return BadRequest(errorModel);
                }
                else
                {
                    errorModel.Message = "خطای نامشخص";
                    return BadRequest(errorModel);
                }
            }
            else
            {
                errorModel.Message = "کد فعالسازی صحیح نمباشد اقدام به ارسال دوباره ی کد بکنید";
                return BadRequest(errorModel);
            }
        }
        [HttpPost("RegisterWithSocial")]
        [ProducesResponseType(typeof(ApiReturn<UserForDetailedDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiReturn<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterWithSocial(UserForRegisterWithSocialDto userForRegisterWithSocialDto)
        {
            var model = new ApiReturn<UserForDetailedDto>
            {
                Status = true
            };

            var user = await _db._UserRepository.GetByIdAsync(userForRegisterWithSocialDto.UserId);
            if (user != null)
            {
                await _db.SaveAcync();

                model.Message = "ورود شما با موفقیت انجام شد";
                model.Result = _mapper.Map<UserForDetailedDto>(user);
                model.Result.IsRegisterBefore = true;
                return CreatedAtRoute("GetUser", new
                {
                    controller = "Users",
                    v = HttpContext.GetRequestedApiVersion().ToString(),
                    id = userForRegisterWithSocialDto.UserId
                }, model);
            }
            else
            {
                var userToCreate = new User
                {
                    UserName = userForRegisterWithSocialDto.Email,
                    Name = userForRegisterWithSocialDto.Name,
                    PhoneNumber = "0000",
                    Address = "",
                    City = "",
                    Gender = true,
                    DateOfBirth = DateTime.Now,
                    IsActive = true,
                    PhoneNumberConfirmed = true
                };
                userToCreate.Id = userForRegisterWithSocialDto.UserId;
               
                var walletMain = new Wallet
                {
                    UserId = userForRegisterWithSocialDto.UserId,
                    Name = "اصلی ماد پی",
                    IsMain = true,
                    IsSms = false,
                    Inventory = 0,
                    InterMoney = 0,
                    ExitMoney = 0,
                    OnExitMoney = 0

                };
                var walletSms = new Wallet
                {
                    UserId = userForRegisterWithSocialDto.UserId,
                    Name = "پیامک",
                    IsMain = false,
                    IsSms = true,
                    Inventory = 0,
                    InterMoney = 0,
                    ExitMoney = 0,
                    OnExitMoney = 0

                };

                var result = await _userManager.CreateAsync(userToCreate, userForRegisterWithSocialDto.Email);
                if (result.Succeeded)
                {
                   // await _authService.AddUserPreNeededAsync( walletMain, walletSms);
                    var creaatedUser = await _userManager.FindByNameAsync(userToCreate.UserName);
                    await _userManager.AddToRolesAsync(creaatedUser, new[] { "User" });
                    var userForReturn = _mapper.Map<UserForDetailedDto>(userToCreate);
                    userForReturn.IsRegisterBefore = false;
                    _logger.LogInformation($"{userForRegisterWithSocialDto.Name} - {userForRegisterWithSocialDto.Email} ثبت نام کرده است");
                    //
                    model.Message = "ورود شما با موفقیت انجام شد";
                    model.Result = userForReturn;
                    return CreatedAtRoute("GetUser", new
                    {
                        controller = "Users",
                        v = HttpContext.GetRequestedApiVersion().ToString(),
                        id = userToCreate.Id
                    }, model);
                }
                else if (result.Errors.Any())
                {
                    _logger.LogWarning(result.Errors.First().Description);
                    //
                    errorModel.Message = result.Errors.First().Description;
                    return BadRequest(errorModel);
                }
                else
                {
                    errorModel.Message = "خطای نامشخص";
                    return BadRequest(errorModel);
                }
            }


        }

    }
}
