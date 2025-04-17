using Models;
using Services;
using Console;
using Services;

class Program
{
    static void Main(string[] args)
    {
        ITaskManager taskManager = new Services.TaskManager();
        IBSUIRScheduleService scheduleService = new Services.BSUIRScheduleService();

        var consoleUi = new ConsoleUI(taskManager, scheduleService);
        consoleUi.Run();
    }
}