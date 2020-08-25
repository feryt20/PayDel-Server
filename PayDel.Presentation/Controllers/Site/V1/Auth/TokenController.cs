using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayDel.Common.Helpers;
using PayDel.Common.Interface;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Token;
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
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ILogger<TokenController> _logger;
        private readonly IUtilities _utilities;
        private readonly UserManager<Data.Models.User> _userManager;
        private readonly TokenSetting _tokenSetting;


        public TokenController(IUnitOfWork<PayDelDbContext> dbContext, IAuthService authService,
            IMapper mapper, ILogger<TokenController> logger, IUtilities utilities,
            UserManager<Data.Models.User> userManager, TokenSetting tokenSetting)
        {
            _db = dbContext;
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
            _utilities = utilities;
            _userManager = userManager;
            _tokenSetting = tokenSetting;
        }


        [HttpPost]
        public async Task<IActionResult> Auth(TokenRequestDto tokenRequestDto)
        {
            switch (tokenRequestDto.GrantType)
            {
                case "password":
                    //return await GenerateNewToken(tokenRequestDto);
                case "refresh_token":
                    //return await RefreshToken(tokenRequestDto);
                default:
                    return Unauthorized("خطا در اعتبار سنجی دوباره");
            }
        }
    }
}
