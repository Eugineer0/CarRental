using System.Web.Http;
using CarRentalApp.Exceptions;
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
                await HandleGeneralExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleGeneralExceptionAsync(HttpContext httpContext, SharedException exception)
        {
            var actionContext = new ActionContext()
            {
                HttpContext = httpContext
            };
                
            switch (exception.ErrorType)
            {
                case ErrorTypes.AuthFailed:
                {
                    var result = new UnauthorizedObjectResult(exception.Message);
                    await result.ExecuteResultAsync(actionContext);
                    break;
                }
                case ErrorTypes.Invalid:
                {
                    var result = new BadRequestObjectResult(exception.Message);
                    await result.ExecuteResultAsync(actionContext);
                    break;
                }
                case ErrorTypes.NotFound:
                {
                    var result = new NotFoundObjectResult(exception.Message);
                    await result.ExecuteResultAsync(actionContext);
                    break;
                }
                case ErrorTypes.Conflict:
                {
                    var result = new ConflictObjectResult(exception.Message);
                    await result.ExecuteResultAsync(actionContext);
                    break;
                }
                default:
                {
                    var result = new InternalServerErrorResult();
                    await result.ExecuteResultAsync(actionContext);
                    break;
                }
            }
        }
    }
}