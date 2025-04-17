using CourseWork.Services;
using Console;


class Program
{
    static void Main(string[] args)
    {
        ITaskManager taskManager = new CourseWork.Services.TaskManager();
        IBSUIRScheduleService scheduleService = new CourseWork.Services.BSUIRScheduleService();

        var consoleUi = new ConsoleUI(taskManager, scheduleService);
        consoleUi.Run();
    }
}