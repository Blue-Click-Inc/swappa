using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Swappa.Server.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public ValidationFilterAttribute()
        {}

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context) 
        {
            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
        }
    }
}
