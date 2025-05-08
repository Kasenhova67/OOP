using CourseWork.Models;
using CourseWork.Services;
using Xunit;

public class TaskManagerTests
{
    private readonly TaskManager _taskManager = new TaskManager();

    [Fact]
    public void AddTask_AddsTaskToCollection()
    {
        var task = new CourseWork.Models.Task("New Task", "Description", DateTime.Now);

        _taskManager.AddTask(task);

        Assert.Single(_taskManager.GetAllTasks());
    }

    [Fact]
    public void AddExam_AddsExamAsTask()
    {
        var exam = new Exam("Biology", "Test", DateTime.Now.AddDays(3),
                          "test", 5, "Room 303");

        _taskManager.AddExam(exam);

        Assert.Single(_taskManager.GetAllExams());
    }

    [Fact]
    public void RemoveTask_RemovesTaskFromCollection()
    {
        var task = new CourseWork.Models.Task("Temp Task", "To be removed", DateTime.Now);
        _taskManager.AddTask(task);

        _taskManager.RemoveTask(task.Id);

        Assert.Empty(_taskManager.GetAllTasks());
    }
}