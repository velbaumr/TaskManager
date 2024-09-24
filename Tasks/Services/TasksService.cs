using System.Linq.Dynamic.Core;
using System.Text;
using Ardalis.GuardClauses;
using Tasks.DataAccess;
using Task = Tasks.Models.Task;
using TaskStatus = Tasks.Models.TaskStatus;

namespace Tasks.Services;

public class TasksService(TasksDbContext context) : ITasksService
{
    public Task? GetById(int id)
    {
        return context.Tasks.Find(id);
    }

    public IEnumerable<Task> GetTasks(string? searchString, bool? isAscending, TaskStatus? statusFilter,
        string sortColumn)
    {
        if (searchString != null)
        {
            return Search(searchString);
        }

        var tasks = statusFilter.HasValue ? Filter(statusFilter.Value) : Sort(isAscending, sortColumn);

        return tasks;
    }

    private IEnumerable<Task> Search(string searchString)
    {
        var keywords = GetKeywords(searchString);

        var titles = context.Tasks
            .Where("t => @0.Any(s => t.Description.ToLower().Contains(s.ToLower()))", keywords).ToList();

        var descriptions = context.Tasks
            .Where("t => @0.Any(s => t.Description.ToLower().Contains(s.ToLower()))", keywords).ToList();

        return titles.Union(descriptions);
    }

    private IEnumerable<Task> Sort(bool? isAscending, string sortColumn)
    {
        var builder = new StringBuilder();

        builder.Append(sortColumn);
        builder.Append(isAscending.HasValue && !isAscending.Value ? " desc" : " asc");

        var tasks = context.Tasks
            .OrderBy(builder.ToString())
            .ToList();
        return tasks;
    }

    private IEnumerable<Task> Filter(TaskStatus statusFilter)
    {
        var builder = new StringBuilder();

        builder.Append("Status == ");
        builder.Append(statusFilter.ToString("D"));

        var tasks = context.Tasks
            .Where(builder.ToString())
            .ToList();
        return tasks;
    }

    public void RemoveTask(Task task)
    {
        GuardInput(task);
        context.Tasks.Remove(task);
        context.SaveChanges();
    }

    public void CreateTask(Task task)
    {
         GuardInput(task);
         context.Add(task);
         context.SaveChanges();
    }

    public void EditTask(Task task)
    {
        GuardInput(task);
        context.Update(task);
        context.SaveChanges();
    }

    private static IEnumerable<string> GetKeywords(string searchSting)
    {
        var cleaned = string.Concat(searchSting.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)));

        return cleaned.Split(" ").Select(x => x.Trim());
    }

    private static void GuardInput(Task task)
    {
        Guard.Against.NullOrWhiteSpace(task.Title);
        Guard.Against.NullOrWhiteSpace(task.Title);
        Guard.Against.Expression(x => x.Date <= DateTime.Today, task.DueDate, string.Empty);
        Guard.Against.EnumOutOfRange(task.Status);
    }
}