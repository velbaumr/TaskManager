using Microsoft.EntityFrameworkCore;
using Task = Tasks.Models.Task;

namespace Tasks.DataAccess;

public class TasksDbContext: DbContext
{
    public TasksDbContext(DbContextOptions<TasksDbContext> options)
        : base(options)
    {
    }

    public DbSet<Task> Tasks { get; set; }
}