namespace TaskService.test;

using System.Linq;
using TaskService.Core;
using TaskService.Core.Repository;

public class TaskServiceTest
{
    private static readonly Guid UserA = Guid.NewGuid();
    private static readonly Guid UserB = Guid.NewGuid();

    [Fact]
    public void AddTask_ShouldAddTaskToList()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);

        // Act
        service.AddTask("Task 1", UserA);

        // Assert
        Assert.Single(service.GetTasks(UserA));
    }

    [Fact]
    public void AddTask_ShouldStoreCorrectTitle()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);

        // Act
        service.AddTask("Task 2", UserA);

        // Assert
        var task = service.GetTasks(UserA).First();
        Assert.Equal("Task 2", task.Title);
    }

    [Fact]
    public void CompleteTask_ShouldMarkAsCompleted()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var task = service.AddTask("Task 3", UserA);

        // Act
        service.CompleteTask(task.Id, UserA);

        // Assert
        Assert.True(service.GetTasks(UserA).First().IsCompleted);
    }

    [Fact]
    public void CompleteTask_ShouldThrow_WhenTaskDoesNotExist()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var id = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => service.CompleteTask(id, UserA));
    }

    [Fact]
    public void DeleteTask_ShouldRemoveTask()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var task = service.AddTask("Task 4", UserA);

        // Act
        service.DeleteTask(task.Id, UserA);

        // Assert
        Assert.Empty(service.GetTasks(UserA));
    }

    [Fact]
    public void GetTasks_ShouldOnlyReturnTasksForThatUser()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        service.AddTask("Task A", UserA);
        service.AddTask("Task B", UserB);

        // Act
        var tasks = service.GetTasks(UserA);

        // Assert
        Assert.Single(tasks);
        Assert.Equal("Task A", tasks.First().Title);
    }

    [Fact]
    public void CompleteTask_ShouldThrow_WhenTaskBelongsToAnotherUser()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var task = service.AddTask("Task A", UserA);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => service.CompleteTask(task.Id, UserB));
        Assert.False(service.GetTasks(UserA).First().IsCompleted);
    }

    [Fact]
    public void DeleteTask_ShouldThrow_WhenTaskBelongsToAnotherUser()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var task = service.AddTask("Task A", UserA);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => service.DeleteTask(task.Id, UserB));
        Assert.Single(service.GetTasks(UserA));
    }

    [Fact]
    public async Task HTTPHealth_ReturnsOk()
    {
        // Arrange
        var client = new HttpClient();

        // Act
        var response = await client.GetAsync("http://localhost:8000/health");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
}