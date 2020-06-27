using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayDel.Data.DatabaseContext;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;

namespace PayDel.Presentation.Controllers.Api
{
    
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class AppiController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public AppiController(IUnitOfWork<PayDelDbContext> dbContext, IAuthService authService, IConfiguration config)
        {
            _db = dbContext;
            _authService = authService;
            _config = config;
        }


        [HttpGet("getval")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var u = await _db._UserRepository.GetAllAsync();
            return Ok(u);
        }
    }
}
