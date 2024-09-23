using Task = Tasks.Models.Task;
using TaskStatus = Tasks.Models.TaskStatus;

namespace Tests.UnitTests;

public static class TestDataHelper
{
    public static List<Task> GetFakeTaskList()
    {
        return
        [
            new Task
            {
                Id = 1,
                Status = TaskStatus.Pending,
                Title = "First Task",
                Description = "First Task Description",
                DueDate = DateTime.Today.AddDays(3)
            },
            new Task
            {
                Id = 2,
                Status = TaskStatus.Completed,
                Title = "Second Task",
                Description = "Second Task Description",
                DueDate = DateTime.Today.AddDays(4)
            },
            new Task
            {
                Id = 3,
                Status = TaskStatus.InProgress,
                Title = "Third Task",
                Description = "Third Task Description",
                DueDate = DateTime.Today.AddDays(5)
            },
            new Task
            {
                Id = 4,
                Status = TaskStatus.Pending,
                Title = "Fourth Task",
                Description = "Fourth Task Description",
                DueDate = DateTime.Today.AddDays(6)
            }
        ];
    }
}