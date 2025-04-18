<<<<<<< HEAD
﻿using CourseWork.Services;
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
=======
﻿using Models;
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
>>>>>>> 1db942a9e8ab65cb57a9decb16eea9788ae7a2f4
}