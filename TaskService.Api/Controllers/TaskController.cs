namespace TaskService.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using TaskService.Api.DTO;

[ApiController]
[Route("tasks")]
public class TaskController : ControllerBase
{
    private readonly Core.TaskService taskManager;

    public TaskController(Core.TaskService taskService)
    {
        taskManager = taskService;
    }

    [HttpGet]
    public IEnumerable<TaskItem> GetTasks()
    {
        return taskManager.GetTasks();
    }

    [HttpPost]
    public TaskItem AddTask([FromBody] CreateTaskRequest request)
    {
        return taskManager.AddTask(request.Title);
    }

    [HttpPatch("{id}/complete")]
    public IActionResult CompleteTask(Guid id)
    {
        taskManager.CompleteTask(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(Guid id)
    {
        taskManager.DeleteTask(id);
        return NoContent();
    }
}