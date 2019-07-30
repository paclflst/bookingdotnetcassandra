using System.Collections.Generic;
using System.Web.Http;

namespace BookingService.Controllers
{
    [Authorize]
    public class TestController : ApiController
    { 
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello REST API", "I am Authorized" };
        }
    }
}
