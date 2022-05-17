using APP.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DomainController : ControllerBase
    {
        [HttpGet]
        public dynamic GetAsync()
        {
          var output = MongoDBConnection.GetConnection();

            if (output != null)
            {
                return Ok(output);
            }
            else
            {
                return BadRequest(output);
            }
        }      
    }
}
