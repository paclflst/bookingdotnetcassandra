using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
namespace BookingService.Helper_Code.Validation
{
    /// <summary>
    /// If there is a model error during executing an action, it returns a Bad Request response with the model details.
    /// </summary>
    public class ReturnBadRequestOnModelError : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }
        }
    }
}