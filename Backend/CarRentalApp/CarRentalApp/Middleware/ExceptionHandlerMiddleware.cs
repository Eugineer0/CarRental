using CarRentalBll.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApp.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (SharedException exception)
            {
                await HandleSharedExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleSharedExceptionAsync(HttpContext httpContext, SharedException exception)
        {
            var actionContext = new ActionContext()
            {
                HttpContext = httpContext
            };

            ActionResult result;

            switch (exception.ErrorType)
            {
                case ErrorTypes.AdditionalDataRequired:
                {
                    result = new ObjectResult(exception.Message)
                    {
                        StatusCode = StatusCodes.Status308PermanentRedirect,
                    };
                    break;
                }
                case ErrorTypes.AuthFailed:
                {
                    result = new UnauthorizedObjectResult(exception.Message);
                    break;
                }
                case ErrorTypes.AccessDenied:
                {
                    result = new ObjectResult(exception.Message) { StatusCode = StatusCodes.Status403Forbidden };
                    break;
                }
                case ErrorTypes.Invalid:
                {
                    result = new BadRequestObjectResult(exception.Message);
                    break;
                }
                case ErrorTypes.NotFound:
                {
                    result = new NotFoundObjectResult(exception.Message);
                    break;
                }
                case ErrorTypes.Conflict:
                {
                    result = new ConflictObjectResult(exception.Message);
                    break;
                }
                default:
                {
                    result = new ObjectResult(exception.Message)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };
                    break;
                }
            }

            await result.ExecuteResultAsync(actionContext);
        }
    }
}