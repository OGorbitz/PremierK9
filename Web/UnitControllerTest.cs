using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitControllerTest : ControllerBase
    {
        // GET: api/<UnitControllerTest>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UnitControllerTest>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UnitControllerTest>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UnitControllerTest>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UnitControllerTest>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
