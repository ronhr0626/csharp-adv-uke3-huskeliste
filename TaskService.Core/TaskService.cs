namespace TaskService.Core;

using global::TaskService.Core.Repository;

public class TaskService
{
    private readonly ITaskRepository repository;

    public TaskService(ITaskRepository repository)
    {
        this.repository = repository;
    }

    public TaskItem AddTask(string title, Guid userId)
    {
        var task = new TaskItem(title, userId);
        repository.Add(task);
        return task;
    }

    public List<TaskItem> GetTasks(Guid userId)
    {
        return repository.GetAll(userId);
    }

    public void CompleteTask(Guid id, Guid userId)
    {
        var task = repository.GetById(id, userId)
            ?? throw new KeyNotFoundException("Task was not found");

        task.IsCompleted = true;
        repository.Update(task);
    }

    public void DeleteTask(Guid id, Guid userId)
    {
        var task = repository.GetById(id, userId)
            ?? throw new KeyNotFoundException("Task was not found");

        repository.Delete(task);
    }
}