using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PayDel.Common.ErrorsAndMessages;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;

namespace PayDel.Presentation.Controllers.Site.Admin
{
    [Route("site/admin/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public AuthController(IUnitOfWork<PayDelDbContext> dbContext, IAuthService authService, IConfiguration config)
        {
            _db = dbContext;
            _authService = authService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            if(await _db._UserRepository.UserExistsAsync(userForRegisterDto.UserName))
            {
                
                    return BadRequest(new ReturnMessage()
                    {
                        status=false,
                        title="خطا",
                        message="نام کاربری قبلا ثبت شده",
                        code="400"
                    });
            }
                

            var userToCreate = new User
            {
                Name = userForRegisterDto.Name,
                UserName = userForRegisterDto.UserName,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                DateOfBith = DateTime.Now,
                IsActive = true,
                Confirmed = true,
            };

            var createdUser = await _authService.Register(userToCreate, userForRegisterDto.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            throw new Exception("nnnnnnnn");
            var userFromRepo = await _authService.Login(userForLoginDto.UserName, userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized(new ReturnMessage()
                {
                    status = false,
                    title = "خطا",
                    message = "نام کاربری یا کلمه عبور اشتباه است"
                });

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = userForLoginDto.IsRemember ? DateTime.Now.AddDays(2) : DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDes);


            return Ok(new
            {
                token=tokenHandler.WriteToken(token)
            });
        }


        [Authorize]
        [HttpGet("getval")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var u = await _db._UserRepository.GetAllAsync();
            return Ok(u);
        }

    }
}
