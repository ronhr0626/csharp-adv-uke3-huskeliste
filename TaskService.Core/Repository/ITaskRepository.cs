namespace TaskService.Core.Repository;

public interface ITaskRepository
{
    void Add(TaskItem task);
    List<TaskItem> GetAll(Guid userId);
    TaskItem? GetById(Guid id, Guid userId);
    void Delete(TaskItem task);
    void Update(TaskItem task);
}