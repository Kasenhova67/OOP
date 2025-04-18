using CourseWork.Models;
using Xunit;

public class TaskTests
{
    [Fact]
    public void Task_Constructor_SetsPropertiesCorrectly()
    {
        
        var task = new CourseWork.Models.Task("Study", "Learn OOP", DateTime.Now.AddDays(1));

        Assert.Equal("Study", task.Title);
        Assert.Equal("Learn OOP", task.Description);
        Assert.False(task.Completed);
        Assert.NotEqual(Guid.Empty, task.Id);
    }

    [Fact]
    public void MarkAsCompleted_SetsCompletedToTrue()
    {
        
        var task = new CourseWork.Models.Task("Test", "Test", DateTime.Now);

        task.MarkAsCompleted();

        Assert.True(task.Completed);
    }

    [Fact]
    public void IsDue_ReturnsTrue_WhenDueDatePassed()
    {
        
        var task = new CourseWork.Models.Task("Past Due", "Test", DateTime.Now.AddDays(-1));

        Assert.True(task.IsDue());
    }
}