namespace Tasks.Models;

public class TaskListViewModel
{
    public TaskStatus? StatusFilter { get; set; }
    public string? SortColumn { get; set; }
    public bool? IsAscending { get; set; }
    public string? Search { get; set; }
    public IEnumerable<Task> Tasks { get; set; } = [];
}