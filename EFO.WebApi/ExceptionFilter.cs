using System.Net;
using EFO.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFO.WebApi;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException domainException)
        {
            context.Result = new ObjectResult(new { message = domainException.Message, errors = domainException.Errors, })
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        else
        {
            string? exceptionInfo = null;
#if DEBUG
            exceptionInfo = context.Exception.ToString();
#endif
            context.Result = new ObjectResult(new { message = "Unknown error occurred.", exception = exceptionInfo })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }
    }
}
