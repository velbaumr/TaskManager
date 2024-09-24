using Microsoft.AspNetCore.Diagnostics;

namespace Tasks.Handlers;

public class TasksExceptionHandler: IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var page = (exception is ArgumentException)? 
            "/StatusCode/BadRequestCode/" :
            "/StatusCode/ServerErrorCode/";

        httpContext.Response.Redirect(page);

        return ValueTask.FromResult(true);
    }
}