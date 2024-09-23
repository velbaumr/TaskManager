using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tasks.DataAccess;
using Task = Tasks.Models.Task;
using TaskStatus = Tasks.Models.TaskStatus;

namespace Tests.IntegrationTests;

public class DataAcessTests
{
    private readonly TasksDbContext _context;
    
    public DataAcessTests()
    {
        var services = new ServiceCollection();

        services.AddDbContext<TasksDbContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

       var serviceProvider = services.BuildServiceProvider();
        _context = serviceProvider.GetService<TasksDbContext>() ?? throw new InvalidOperationException();
    }

    private void SetupTestData()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.Tasks.AddRange(TestDataHelper.GetFakeTaskList());
        _context.SaveChanges();
    }

   
    [Fact]
    public void ShouldGetAllTasks()
    {
        SetupTestData();
        var result = _context.Tasks.ToList();
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public void ShouldAddTask()
    {
        SetupTestData();
        _context.Add(new Task
        {
            DueDate = DateTime.Today.AddDays(1),
            Title = "Test",
            Description = "Test",
            Status = TaskStatus.Pending
        });
        _context.SaveChanges();
        Assert.Equal(5, _context.Tasks.AsEnumerable().Count());
    }

    [Fact]
    public void ShouldRemoveTask()
    {
        SetupTestData();
        var toRemove = _context.Tasks.Find(1);
        _context.Remove(toRemove);
        _context.SaveChanges();
        Assert.Equal(3, _context.Tasks.AsEnumerable().Count());
    }
}