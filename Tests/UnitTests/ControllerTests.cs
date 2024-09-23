using Microsoft.AspNetCore.Mvc;
using Moq;
using Tasks.Controllers;
using Tasks.Services;
using Task = Tasks.Models.Task;


namespace Tests.UnitTests;

public class ControllerTests
{
    [Fact]
    public void DeleteConfirmedShouldCallTaskService()
    {
        const int id = 1;
        var task = TestDataHelper.GetFakeTaskList()[0];
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(id))
            .Returns(task);

        var controller = new TaskController(serviceMock.Object);
        controller.DeleteConfirmed(id);

        serviceMock.Verify(x => x.RemoveTask(task), Times.Once);
    }
    
    [Fact]
    public void IndexShouldCallTaskService()
    {
        var serviceMock = new Mock<ITasksService>();
        
        var controller = new TaskController(serviceMock.Object);
        controller.Index(null, true, null);
        
        serviceMock.Verify(x => x.GetTasks(null, true, null, "DueDate"), Times.Once);
    }
    
    [Fact]
    public void PostingCreateShouldCallTaskService()
    {
        var task = TestDataHelper.GetFakeTaskList()[0];
        var serviceMock = new Mock<ITasksService>();
        

        var controller = new TaskController(serviceMock.Object);
        controller.Create(task);

        serviceMock.Verify(x => x.CreateTask(task), Times.Once);
    }
    
    [Fact]
    public void PostingEditShouldCallTaskService()
    {
        var task = TestDataHelper.GetFakeTaskList()[0];
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(1))
            .Returns(task);

        var controller = new TaskController(serviceMock.Object);
        controller.Edit(1, task);

        serviceMock.Verify(x => x.EditTask(task), Times.Once);
    }

    [Fact]
    public void DeleteShouldReturnNotFoundWhenIdIsWrong()
    {
        const int id = 200;
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(id))
            .Returns((Task?)null);
        
        var controller = new TaskController(serviceMock.Object);
        var result = controller.Delete(id);

        Assert.True(result is NotFoundResult);
    }
    
    [Fact]
    public void DeleteShouldReturnViewWhenIdExists()
    {
        const int id = 1;
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(id))
            .Returns(TestDataHelper.GetFakeTaskList()[0]);
        
        var controller = new TaskController(serviceMock.Object);
        var result = controller.Delete(id);

        Assert.True(result is ViewResult);
    }
    
    [Fact]
    public void EditShouldReturnNotFoundWhenIdIsWrong()
    {
        const int id = 200;
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(id))
            .Returns((Task?)null);
        
        var controller = new TaskController(serviceMock.Object);
        var result = controller.Edit(id);

        Assert.True(result is NotFoundResult);
    }
    
    [Fact]
    public void EditShouldReturnViewWhenIdExists()
    {
        const int id = 1;
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(id))
            .Returns(TestDataHelper.GetFakeTaskList()[0]);
        
        var controller = new TaskController(serviceMock.Object);
        var result = controller.Edit(id);

        Assert.True(result is ViewResult);
    }

    [Fact]
    public void PostingEditShouldRedirectToIndex()
    {
        const int id = 1;
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(id))
            .Returns(TestDataHelper.GetFakeTaskList()[0]);
        var controller = new TaskController(serviceMock.Object);
        var result = controller.Edit(id, TestDataHelper.GetFakeTaskList()[0]);
        
        var action = ((RedirectToActionResult)result).ActionName;
        Assert.Equal(nameof(Index), action);
        
    }
    
    [Fact]
    public void PostingCreateShouldRedirectToIndex()
    {
        
        var serviceMock = new Mock<ITasksService>();
        var controller = new TaskController(serviceMock.Object);
        var result = controller.Create(new Task());

        var action = ((RedirectToActionResult)result).ActionName;
        Assert.Equal(nameof(Index), action);
    }
    
    [Fact]
    public void PostingDeleteConfirmedShouldRedirectToIndex()
    {
        const int id = 1;
        var serviceMock = new Mock<ITasksService>();
        serviceMock.Setup(x => x.GetById(id))
            .Returns(TestDataHelper.GetFakeTaskList()[0]);
        var controller = new TaskController(serviceMock.Object);
        var result = controller.DeleteConfirmed(id);

        var action = ((RedirectToActionResult)result).ActionName;
        Assert.Equal(nameof(Index), action);
    }
    
    [Fact]
    public void ShouldRejectTodaysDate()
    {
        var serviceMock = new Mock<ITasksService>();
        var controller = new TaskController(serviceMock.Object);
        var result = controller.Create(new Task
        {
            DueDate = DateTime.Today
        });

        var action = ((RedirectToActionResult)result).ActionName;
        Assert.Equal(nameof(Index), action);
    }
}