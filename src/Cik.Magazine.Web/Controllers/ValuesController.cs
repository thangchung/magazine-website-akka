using System.Collections.Generic;
using System.Threading.Tasks;
using Akka.Actor;
using Cik.Magazine.Core;
using Cik.Magazine.Core.Domain;
using Cik.Magazine.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cik.Magazine.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly CategoryService _service;

        public ValuesController(CategoryService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _service.DoSomething();
            return new[] {"value1", "value2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}