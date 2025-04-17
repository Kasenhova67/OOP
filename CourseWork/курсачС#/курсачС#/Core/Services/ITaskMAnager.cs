using CourseWork.Models;


namespace CourseWork.Services
{
    public interface ITaskManager
    {
        void AddTask(CourseWork.Models.Task task);
        void AddExam(Exam exam);
        IEnumerable<CourseWork.Models.Task> GetAllTasks();
        IEnumerable<Exam> GetAllExams();
        CourseWork.Models.Task GetTaskById(Guid id);
        void MarkTaskAsCompleted(Guid id);
        void RemoveTask(Guid id);
    }
}