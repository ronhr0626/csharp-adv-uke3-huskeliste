using Microsoft.AspNetCore.Mvc;
using TaskService.Api.DTO;
using TaskService.Api.Services;

namespace TaskService.Api.Controllers;

[ApiController]
[Route("tasks")]
public class TaskController : ControllerBase
{
    private readonly Core.TaskService taskManager;
    private readonly ITokenReader tokenReader;

    public TaskController(Core.TaskService taskService, ITokenReader tokenReader)
    {
        taskManager = taskService;
        this.tokenReader = tokenReader;
    }

    private Guid? GetUserId()
    {
        return tokenReader.GetUserId(Request.Headers.Authorization.ToString());
    }

    [HttpGet]
    public IActionResult GetTasks()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        return Ok(taskManager.GetTasks(userId.Value));
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] CreateTaskRequest request)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        return Ok(taskManager.AddTask(request.Title, userId.Value));
    }

    [HttpPatch("{id}/complete")]
    public IActionResult CompleteTask(Guid id)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        // The domain decides if the task exists for this user. The controller only maps it to HTTP.
        try
        {
            taskManager.CompleteTask(id, userId.Value);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTask(Guid id)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            taskManager.DeleteTask(id, userId.Value);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
