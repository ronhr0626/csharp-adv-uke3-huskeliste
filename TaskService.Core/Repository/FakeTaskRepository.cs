namespace TaskService.Core.Repository;

public class FakeTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> tasks = new();

    public void Add(TaskItem task)
    {
        tasks.Add(task);
    }

    public List<TaskItem> GetAll(Guid userId)
    {
        return tasks.Where(t => t.UserId == userId).ToList();
    }

    public TaskItem? GetById(Guid id, Guid userId)
    {
        return tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
    }

    public void Delete(TaskItem task)
    {
        tasks.Remove(task);
    }

    public void Update(TaskItem task)
    {
        var existingTask = tasks.FirstOrDefault(t => t.Id == task.Id && t.UserId == task.UserId)
            ?? throw new KeyNotFoundException("Task was not found");

        tasks[tasks.IndexOf(existingTask)] = task;
    }
}