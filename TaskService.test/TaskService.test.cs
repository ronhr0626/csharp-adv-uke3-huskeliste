namespace TaskService.test;

using System.Linq;
using TaskService.Core;
using TaskService.Core.Repository;
public class TaskServiceTest
{
    [Fact]
    public void AddTask_ShouldAddTaskToList()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);

        // Act
        service.AddTask("Task 1");

        // Assert
        Assert.Single(service.GetTasks());
    }
    [Fact]
    public void AddTask_ShouldStoreCorrectTitle()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);

        // Act
        service.AddTask("Task 2");

        // Assert
        var task = service.GetTasks().First();
        Assert.Equal("Task 2", task.Title);
    }
    [Fact]
    public void CompleteTask_ShouldMarkAsCompleted()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var task = service.AddTask("Task 3");
    
        // Act
        service.CompleteTask(task.Id);
    
        // Assert
        Assert.True(service.GetTasks().First().IsCompleted);
    }
    [Fact]
    public void CompleteTask_ShouldThrow_WhenTaskDoesNotExist()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var id = Guid.NewGuid();
    
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(()=> service.CompleteTask(id));
    }
    [Fact]
    public void DeleteTask_ShouldRemoveTask()
    {
        // Arrange
        var repository = new FakeTaskRepository();
        var service = new TaskService(repository);
        var task = service.AddTask("Task 4");
    
        // Act
        service.DeleteTask(task.Id);
    
        // Assert
        Assert.Empty(service.GetTasks());
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
