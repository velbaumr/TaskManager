using Microsoft.AspNetCore.Mvc;
using Tasks.Models;

namespace Tasks.Controllers;

public class StatusCodeController : Controller
{
    public IActionResult NotFoundCode()
    {
        var model = new StatusCodeViewModel
        {
            Code = 404,
            Description = "Not Found"
        };

        return View("StatusCode", model);
    }

    public IActionResult BadRequestCode()
    {
        var model = new StatusCodeViewModel
        {
            Code = 400,
            Description = "Bad Request"
        };

        return View("StatusCode", model);
    }

    public IActionResult ServerErrorCode()
    {
        var model = new StatusCodeViewModel
        {
            Code = 500,
            Description = "Server Error"
        };

        return View("StatusCode", model);
    }
}