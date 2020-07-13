using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Repo.Infrastructures;
using PayDel.Services.Site.Admin.Auth.Interface;

namespace PayDel.Presentation.Controllers.Site.Admin
{
    [Authorize]
    [Route("site/admin/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        private readonly IMapper _mapper;
        public UsersController(IUnitOfWork<PayDelDbContext> dbContext, IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db._UserRepository.GetAllAsync(null, null, "Photos,BankCards");

            var userProfile = _mapper.Map<IEnumerable<UserProfileDto>>(users);
            return Ok(userProfile);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(string id)
        {
            if(User.FindFirst(ClaimTypes.NameIdentifier).Value == id)
            {
                var user = await _db._UserRepository.GetAllAsync(p => p.Id == id, null, "Photos,BankCards");
                //var userProfile = _mapper.Map<UserProfileDto>(user);// The Statement Above Returns IEnumerable But We Need Single User
                var userProfile = _mapper.Map<IEnumerable<UserProfileDto>>(user);
                return Ok(userProfile);
            }
            else
            {
                return Unauthorized("شما به این اطلاعات دسترسی ندارید");
            }
        }
    }
}
