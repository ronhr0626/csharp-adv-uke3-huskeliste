using TaskService.Api.Data;
using TaskService.Core.Repository;

namespace TaskService.Api.Repositories;

public class EfTaskRepository : ITaskRepository
{
    private readonly TaskDbContext _db;

    public EfTaskRepository(TaskDbContext db)
    {
        _db = db;
    }

    public void Add(TaskItem task)
    {
        _db.Tasks.Add(task);
        _db.SaveChanges();
    }

    public List<TaskItem> GetAll(Guid userId)
    {
        return _db.Tasks
            .Where(t => t.UserId == userId)
            .ToList();
    }

    public TaskItem? GetById(Guid id, Guid userId)
    {
        return _db.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == userId);
    }

    public void Delete(TaskItem task)
    {
        _db.Tasks.Remove(task);
        _db.SaveChanges();
    }

    public void Update(TaskItem task)
    {
        _db.Tasks.Update(task);
        _db.SaveChanges();
    }
}