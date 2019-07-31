using System.Web;
using System.Web.Http;
using BookingService.Helper_Code.Validation;

namespace BookingService
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new ValidateActionParametersAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new ReturnBadRequestOnModelError());
        }
    }
}
