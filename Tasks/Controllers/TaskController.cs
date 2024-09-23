using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tasks.Models;

namespace Tasks.Controllers;

public class TaskController : Controller
{

    public TaskController()
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}