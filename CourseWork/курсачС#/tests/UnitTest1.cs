using Console;
using CourseWork.Services;
using Moq;
using Xunit;

public class ConsoleUITests
{
    private readonly Mock<ITaskManager> _mockTaskManager = new Mock<ITaskManager>();
    private readonly Mock<IBSUIRScheduleService> _mockScheduleService = new Mock<IBSUIRScheduleService>();
    private readonly ConsoleUI _consoleUi;

    public ConsoleUITests()
    {
        _consoleUi = new ConsoleUI(_mockTaskManager.Object, _mockScheduleService.Object);
    }

    [Fact]
    public void AddTask_CallsTaskManagerAddTask()
    {
        var task = new CourseWork.Models.Task("Test", "Test", DateTime.Now);

        using (var input = new StringReader("Test\nTest\nn\n2023-12-31 23:59"))
        {
            System.Console.SetIn(input);
            _consoleUi.AddTask();
        }

        _mockTaskManager.Verify(m => m.AddTask(It.IsAny<CourseWork.Models.Task>()), Times.Once);
    }
}