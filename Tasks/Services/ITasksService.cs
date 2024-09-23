namespace Tasks.Services;
using Task = Tasks.Models.Task;
using TaskStatus = Tasks.Models.TaskStatus;

public interface ITasksService
{
    Task? GetById(int id);
    IEnumerable<Task> GetTasks(string? searchString, bool? isAscending, TaskStatus? statusFilter, string sortColumn);

    void RemoveTask(Task task);

    void CreateTask(Task task);

    void EditTask(Task task);
};