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
using PayDel.Common.Interface;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;

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
        private readonly IUtilities _utilities;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthController(IUnitOfWork<PayDelDbContext> dbContext, IAuthService authService, 
            IMapper mapper, ILogger<AuthController> logger, IUtilities utilities,
            UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = dbContext;
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
            _utilities = utilities;
            _userManager = userManager;
            _signInManager = signInManager;
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
                    token = _utilities.GenerateJwtToken(appUser, userForLoginDto.IsRemember),
                    user = userForReturn
                });
            }
            else
            {
                _logger.LogWarning($"{userForLoginDto.UserName} درخواست لاگین ناموفق داشته است");
                return Unauthorized("کاربری با این یوزر و پس وجود ندارد");
            }
        }


        [HttpGet("getval")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var u = await _db._UserRepository.GetAllAsync();
            return Ok(u);
        }

    }
}
