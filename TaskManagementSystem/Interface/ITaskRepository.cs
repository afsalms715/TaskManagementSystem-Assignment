using TaskManagementSystem.Models;

namespace TaskManagementSystem.Interface
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskModel>> GetAllTasks();
        Task<TaskModel> CreateNewTask();

    }
}
