using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tasks.Models;
using Tasks.Services;
using Task = Tasks.Models.Task;
using TaskStatus = Tasks.Models.TaskStatus;

namespace Tasks.Controllers;

public class TaskController(ITasksService tasksService) : Controller
{
    public IActionResult Index(string? search, bool? isAscending, TaskStatus? statusFilter,
        string sortColumn = "DueDate")
    {
        var tasks = tasksService.GetTasks(search, isAscending, statusFilter, sortColumn);
        var model = new TaskListViewModel
        {
            SortColumn = sortColumn,
            IsAscending = isAscending,
            Tasks = tasks
        };

        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Task task)
    {
        if (!ModelState.IsValid) return View(task);
        tasksService.CreateTask(task);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        return GetTaskView(id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Task task)
    {
        if (id != task.Id) return NotFound();

        if (!ModelState.IsValid) return View(task);
        tasksService.EditTask(task);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        return GetTaskView(id);
    }

    private IActionResult GetTaskView(int id)
    {
        var task = tasksService.GetById(id);
        if (task == null) return NotFound();
        return View(task);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var task = tasksService.GetById(id);
        if (task == null) return NotFound();
        tasksService.RemoveTask(task);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}