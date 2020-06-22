using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayDel.Data.DatabaseContext;
using PayDel.Data.Infrastructures;
using PayDel.Data.Models;

namespace PayDel.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWork<PayDelDbContext> _db;
        public ValuesController(IUnitOfWork<PayDelDbContext> dbContext)
        {
            _db = dbContext;
        }

        // GET api/val
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            //var user = new User
            //{
            //    Name="Babak",
            //    UserName = "Babak",
            //    PhoneNumber = "0921",
            //    PasswordHash = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
            //    PasswordSalt = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 },
            //    IsActive = true,
            //    Confirmed = true,
            //};

            //await _db._UserRepository.InsertAsync(user);
            //await _db.SaveAcync();

            //var model = await _db._UserRepository.GetAllAsync();

            return Ok("ok ok");
        }

        // GET api/val/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            return "value";
        }
        // POST api/values
        [HttpPost]
        public async Task<string> Post([FromBody] string value)
        {
            return null;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<string> Put(int id, [FromBody] string value)
        {
            return null;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            return null;
        }

    }
}
