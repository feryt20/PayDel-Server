using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PayDel.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        // GET api/val
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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
