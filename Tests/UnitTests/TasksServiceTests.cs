using MockQueryable;
using MockQueryable.Moq;
using Moq;
using Tasks.DataAccess;
using Tasks.Services;
using TaskStatus = Tasks.Models.TaskStatus;

namespace Tests.UnitTests;

public class TasksServiceTests
{
    [Theory]
    [
        InlineData(TaskStatus.Pending, 2),
        InlineData(TaskStatus.InProgress, 1),
        InlineData(TaskStatus.Completed, 1)
    ]
    public void ShouldFilterByStatus(TaskStatus status, int count)
    {
        var contextMock = SetupContextMock();

        var service = new TasksService(contextMock.Object);
        var result = service.GetTasks(null, null, status, "");

        Assert.Equal(count, result.Count());
    }

    [Theory]
    [
        InlineData("DueDate", true, new [] {1, 2, 3, 4}),
        InlineData("DueDate", false, new[] {4, 3, 2, 1}),
        InlineData("Description", true, new [] {1, 4, 2, 3}),
        InlineData("Description", false, new [] {3, 2, 4, 1})
    ]
    public void ShouldSort(string sortColumn, bool? isAscending, IEnumerable<int> expected)
    {
        var contextMock = SetupContextMock();

        var service = new TasksService(contextMock.Object);
        var result = service.GetTasks(null, isAscending, null, sortColumn)
            .Select(x => x.Id)
            .ToList();
        
        Assert.True(result.SequenceEqual(expected));
    }

    [Fact]
    public void ShouldSearchByKeywords()
    {
        var contextMock = SetupContextMock();

        const string searchString = "first second";
        var service = new TasksService(contextMock.Object);
        var result = service.GetTasks(searchString, null, null, string.Empty);

        Assert.Equal(2, result.Count());
    }

    private static  Mock<TasksDbContext> SetupContextMock()
    {
        var mock = TestDataHelper.GetFakeTaskList().BuildMock().BuildMockDbSet();
        var contextMock = new Mock<TasksDbContext>();
        contextMock.Setup(x => x.Tasks).Returns(mock.Object);

        return contextMock;
    }
}