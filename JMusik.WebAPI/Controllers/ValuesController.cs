using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMusik.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //Get api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() => new string[] { "Value1", "Value2" };

        //Get api/values/{id}
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id) => "Value";

        //Post api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        //Put api/values/5
        [HttpPut("{id}")]
        public void Put(int id,[FromBody]string value)
        {
        }

        //Delete api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
