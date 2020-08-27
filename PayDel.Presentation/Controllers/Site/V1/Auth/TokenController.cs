using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PayDel.Common.Helpers;
using PayDel.Common.Interface;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Data.Dtos.Token;
using PayDel.Data.Models;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;

namespace PayDel.Presentation.Controllers.Site.V1.Auth
{
    [Route("v1/site/auth/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class TokenController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IMapper _mapper;
        private readonly ILogger<TokenController> _logger;
        private readonly IUtilities _utilities;


        public TokenController(IUnitOfWork<PayDelDbContext> dbContext,
             IMapper mapper, ILogger<TokenController> logger, IUtilities utilities
            )
        {
            _db = dbContext;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(TokenRequestDto tokenRequestDto)
        {
            switch (tokenRequestDto.GrantType)
            {

                case "password":
                    var result = await _utilities.GenerateNewTokenAsync(tokenRequestDto, true);
                    if (result.status)
                    {
                        //var userForReturn = _mapper.Map<UserForDetailedDto>(result.user);
                        //userForReturn.Provider = tokenRequestDto.Provider;
                        return Ok(new LoginResponseDto
                        {
                            token = result.token,
                            refresh_token = result.refresh_token
                            //user = userForReturn
                        });
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDto.UserName} درخواست لاگین ناموفق داشته است" + "---" + result.message);
                        return Unauthorized("1x111keyvanx11");
                    }
                case "social":
                    var socialresult = await _utilities.GenerateNewTokenAsync(tokenRequestDto, false);
                    if (socialresult.status)
                    {
                        //var userForReturn = _mapper.Map<UserForDetailedDto>(socialresult.user);
                        //userForReturn.Provider = tokenRequestDto.Provider;
                        return Ok(new LoginResponseDto
                        {
                            token = socialresult.token,
                            refresh_token = socialresult.refresh_token
                            //user = userForReturn
                        });
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDto.UserName} درخواست لاگین ناموفق داشته است" + "---" + socialresult.message);
                        return Unauthorized("1x111keyvanx11");
                    }
                case "refresh_token":
                    var res = await _utilities.RefreshAccessTokenAsync(tokenRequestDto);
                    if (res.status)
                    {
                        return Ok(res);
                    }
                    else
                    {
                        _logger.LogWarning($"{tokenRequestDto.UserName} درخواست لاگین ناموفق داشته است" + "---" + res.message);
                        return Unauthorized("0x000keyvanx00");
                    }
                default:
                    return Unauthorized("خطا در اعتبار سنجی دوباره");
            }
        }
    }
}
