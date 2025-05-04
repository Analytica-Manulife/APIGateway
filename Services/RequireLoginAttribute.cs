using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiGateway.Services;

public class RequireLoginAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var sessionService = context.HttpContext.RequestServices.GetService(typeof(SessionService)) as SessionService;

        if (sessionService == null || !sessionService.IsLoggedIn())
        {
            context.Result = new UnauthorizedObjectResult("User must be logged in.");
        }

        base.OnActionExecuting(context);
    }
}