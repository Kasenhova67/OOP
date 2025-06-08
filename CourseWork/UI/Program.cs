using CourseWork.Models;
using CourseWork.Serialization;
using CourseWork.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;


var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var format = config["Serialization:Format"];

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHttpClient<IBSUIRScheduleService, BSUIRScheduleService>();

        ISerializer<CourseWork.Models.Task> serializer = format switch
        {
            "Xml" => new XmlSerializer<CourseWork.Models.Task>(),
            _ => new JsonSerializer<CourseWork.Models.Task>()
        };

        services.AddSingleton(serializer);

        services.AddSingleton<ITaskManager>(provider =>
            new TaskManager(
                provider.GetRequiredService<ISerializer<CourseWork.Models.Task>>(),
                format == "Xml" ? "tasks.xml" : "tasks.json"
            ));
    })
    .Build();
var scheduleService = host.Services.GetRequiredService<IBSUIRScheduleService>();
var taskManager = host.Services.GetRequiredService<ITaskManager>();
/*var googleCalendarService = host.Services.GetRequiredService<IGoogleCalendarService>(); // Добавляем эту строку
**//*//*async System.Threading.Tasks.Task ImportFromGoogleCalendar(ITaskManager taskManager, IGoogleCalendarService googleCalendarService)
    {
        try
        {
            Console.WriteLine("Получаем события из Google Calendar...");
            var events = await googleCalendarService.GetUpcomingEventsAsync();

            if (events.Any())
            {
                Console.WriteLine($"Найдено {events.Count} событий:");
                foreach (var ev in events)
                {
                    Console.WriteLine($"- {ev.Summary} ({ev.Start:dd.MM.yyyy HH:mm})");
                    taskManager.AddGoogleCalendarEvent(ev);
                }
                Console.WriteLine("Все события успешно импортированы как задачи!");
            }
            else
            {
                Console.WriteLine("Событий не найдено.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении событий: {ex.Message}");
        }
    }
*/
/*    async System.Threading.Tasks.Task ViewGoogleCalendarEvents(IGoogleCalendarService googleCalendarService)
    {
        try
        {
            Console.WriteLine("Получаем события из Google Calendar...");
            var events = await googleCalendarService.GetUpcomingEventsAsync(20);

            Console.WriteLine("\nПредстоящие события:");
            Console.WriteLine("=================================");

            if (events.Any())
            {
                foreach (var ev in events)
                {
                    Console.WriteLine(ev);
                }
            }
            else
            {
                Console.WriteLine("Событий не найдено.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении событий: {ex.Message}");
        }
    }*/
    while (true)
{
    Console.Clear();
    Console.WriteLine("=== Учебный Органайзер БГУИР ===");
    Console.WriteLine("1. Просмотреть расписание");
    Console.WriteLine("2. Добавить задание");
    Console.WriteLine("3. Добавить экзамен");
    Console.WriteLine("4. Импортировать экзамены из API БГУИР");
    Console.WriteLine("5. Просмотреть все задания");
    Console.WriteLine("6. Просмотреть все экзамены");
    Console.WriteLine("7. Отметить задание выполненным");
    Console.WriteLine("8. Удалить задание");
    Console.WriteLine("9. Сохранить задачи в файл");
    Console.WriteLine("10. Загрузить задачи из файла");
    Console.WriteLine("11. Очистить просроченные задачи");
    /* Console.WriteLine("9. Импортировать события из Google Calendar");
     Console.WriteLine("10. Просмотреть события Google Calendar");*/
    Console.WriteLine("0. Выход");
    Console.Write("Выберите действие: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1": 
                await ShowSchedule(scheduleService);
                break;

            case "2": 
                AddRegularTask(taskManager);
                break;

            case "3": 
                AddExam(taskManager);
                break;
            case "4":
                await AddExamsFromApi(taskManager, scheduleService);
                break;

            case "5": 
                ShowAllTasks(taskManager);
                break;

            case "6": 
                ShowAllExams(taskManager);
                break;

            case "7": 
                MarkTaskCompleted(taskManager);
                break;

            case "8": 
                RemoveTask(taskManager);
                break;
            // В вызове методов исправляем опечатку и добавляем using для Models
            case "9": // Сохранить задачи
                await SaveToFileAsync(
                    taskManager,
                    format == "Xml"
                        ? new XmlSerializer<CourseWork.Models.Task>()
                        : new JsonSerializer<CourseWork.Models.Task>()
                );
                break;

            case "10": // Загрузить задачи
                await LoadFromFileAsync(
                    taskManager,
                    format == "Xml"
                        ? new XmlSerializer<CourseWork.Models.Task>()
                        : new JsonSerializer<CourseWork.Models.Task>()
                );
                break;
            case "11":
                if (taskManager is TaskManager tm)
                {
                    tm.RemoveExpiredTasks();
                    Console.WriteLine("Просроченные задачи удалены.");
                }
                break;

                /* case "9": 
                     await SaveToFileAsync(taskManager);
                     break;

                 case "10": 
                     await LoadFromFileAsync(taskManager);
                     break;*/
                /* case "9": // Импорт событий из Google Calendar
                     await ImportFromGoogleCalendar(taskManager, googleCalendarService);
                     break;
                 case "10": // Просмотр событий Google Calendar
                     await ViewGoogleCalendarEvents(googleCalendarService);*/
                break;


            case "0": // Выход
                return;

            default:
                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Произошла ошибка: {ex.Message}");
    }

    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
    Console.ReadKey();
}
 /*static async System.Threading.Tasks.Task SaveToFileAsync(ITaskManager taskManager)
{
    Console.WriteLine("\nВыберите формат файла:");
    Console.WriteLine("1. JSON");
    Console.WriteLine("2. XML");
    Console.Write("Ваш выбор: ");

    var formatChoice = Console.ReadLine();
    string filePath, extension;
    ISerializer<CourseWork.Models.Task> serializer;

    switch (formatChoice)
    {
        case "1":
            extension = "json";
            serializer = new JsonSerializer<CourseWork.Models.Task>();
            break;
        case "2":
            extension = "xml";
            serializer = new XmlSerializer<CourseWork.Models.Task>();
            break;
        default:
            Console.WriteLine("Неверный выбор формата.");
            return;
    }

    Console.Write($"Введите имя файла (без расширения .{extension}): ");
    var fileName = Console.ReadLine();
    filePath = $"{fileName}.{extension}";

    try
    {
        var tasks = taskManager.GetAllTasks().ToList();
        await System.Threading.Tasks.Task.Run(() => serializer.Serialize(tasks, filePath));
        Console.WriteLine($"Задачи успешно сохранены в файл {filePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
    }
}

 static async System.Threading.Tasks.Task LoadFromFileAsync(ITaskManager taskManager)
{
    Console.WriteLine("\nВыберите формат файла:");
    Console.WriteLine("1. JSON");
    Console.WriteLine("2. XML");
    Console.Write("Ваш выбор: ");

    var formatChoice = Console.ReadLine();
    string extension;
    ISerializer<CourseWork.Models.Task> serializer;

    switch (formatChoice)
    {
        case "1":
            extension = "json";
            serializer = new JsonSerializer<CourseWork.Models.Task>();
            break;
        case "2":
            extension = "xml";
            serializer = new XmlSerializer<CourseWork.Models.Task>();
            break;
        default:
            Console.WriteLine("Неверный выбор формата.");
            return;
    }

    Console.Write($"Введите имя файла (с расширением .{extension}): ");
    var filePath = Console.ReadLine();

    if (!File.Exists(filePath))
    {
        Console.WriteLine("Файл не найден.");
        return;
    }

    try
    {
        var tasks = await System.Threading.Tasks.Task.Run(() => serializer.Deserialize(filePath)?.ToList() ?? new List<CourseWork.Models.Task>());

        Console.WriteLine("Как загрузить задачи?");
        Console.WriteLine("1. Заменить текущие задачи");
        Console.WriteLine("2. Добавить к существующим");
        Console.Write("Ваш выбор: ");
        var loadChoice = Console.ReadLine();

        if (loadChoice == "1" && taskManager is TaskManager concreteManager)
        {
            concreteManager.ClearAllTasks();
        }

        foreach (var task in tasks)
        {
            if (task is Exam exam)
                taskManager.AddExam(exam);
            else
                taskManager.AddTask(task);
        }

        Console.WriteLine($"Успешно загружено {tasks.Count} задач из файла {filePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
    }

}*/
async  System.Threading.Tasks.Task AddExamsFromApi(ITaskManager taskManager, IBSUIRScheduleService scheduleService)
{
    Console.Write("Введите номер группы (например, 353504): ");
    var groupId = Console.ReadLine();

    try
    {
        Console.WriteLine("Получаем экзамены из API БГУИР...");
        var exams = await scheduleService.GetExamsForGroupAsync(groupId);

        if (exams.Any())
        {
            Console.WriteLine($"Найдено {exams.Count} экзаменов:");
            foreach (var exam in exams)
            {
                Console.WriteLine($"- {exam.Title} ({exam.DueDate:dd.MM.yyyy HH:mm})");
                taskManager.AddExam(exam);
            }
            Console.WriteLine("Все экзамены успешно добавлены!");
        }
        else
        {
            Console.WriteLine("Для указанной группы экзамены не найдены.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при получении экзаменов: {ex.Message}");
    }
}
async System.Threading.Tasks.Task ShowSchedule(IBSUIRScheduleService scheduleService)
{
    Console.Write("Введите номер группы (например, 951001): ");
    var groupId = Console.ReadLine();

    Console.Write("Введите дату (мм.дд.гггг) или оставьте пустым для сегодня: ");
    var dateInput = Console.ReadLine();
    var date = string.IsNullOrEmpty(dateInput) ? DateTime.Today : DateTime.Parse(dateInput);

    var lessons = await scheduleService.GetScheduleForGroupAsync(groupId, date);

    Console.WriteLine($"\nРасписание для группы {groupId} на {date:dd.MM.yyyy}:");
    Console.WriteLine("=================================");

    if (lessons.Any())
    {
        foreach (var lesson in lessons)
        {
            Console.WriteLine(lesson);
        }
    }
    else
    {
        Console.WriteLine("Занятий не найдено.");
    }
}

void AddRegularTask(ITaskManager taskManager)
{
    Console.Write("Введите название задания: ");
    var title = Console.ReadLine();

    Console.Write("Введите описание: ");
    var description = Console.ReadLine();

    Console.Write("Введите срок выполнения (мм.дд.гггг чч:мм): ");
    var dueDate = DateTime.Parse(Console.ReadLine());

    if (dueDate < DateTime.Now)
    {
        Console.WriteLine("Ошибка: Нельзя создать задачу с прошедшей датой.");
        return;
    }

    Console.Write("Связано с занятием? (y/n): ");
    var related = Console.ReadLine().ToLower() == "y";

    string lessonTitle = null;
    if (related)
    {
        Console.Write("Введите название занятия: ");
        lessonTitle = Console.ReadLine();
    }

    var task = new CourseWork.Models.Task(title, description, dueDate, related, lessonTitle);
    taskManager.AddTask(task);

    Console.WriteLine("Задание успешно добавлено!");
}

void AddExam(ITaskManager taskManager)
{
    Console.Write("Введите название экзамена: ");
    var title = Console.ReadLine();

    Console.Write("Введите описание: ");
    var description = Console.ReadLine();

    Console.Write("Введите дату экзамена (мм.дд.гггг чч:мм): ");
    var dueDate = DateTime.Parse(Console.ReadLine());

    if (dueDate < DateTime.Now)
    {
        Console.WriteLine("Ошибка: Экзамен не может быть в прошлом.");
        return;
    }

    Console.Write("Тип экзамена (зачет/экзамен): ");
    var examType = Console.ReadLine();

    Console.Write("Время на подготовку (в часах): ");
    var prepTime = int.Parse(Console.ReadLine());

    Console.Write("Место проведения: ");
    var location = Console.ReadLine();

    var exam = new Exam(title, description, dueDate, examType, prepTime, location);
    taskManager.AddExam(exam);

    Console.WriteLine("Экзамен успешно добавлен!");
}
/*
static async System.Threading.Tasks.Task SaveToFileAsync(ITaskManager taskManager)
{
    Console.WriteLine("\nВыберите место сохранения:");
    Console.WriteLine("1. Локально");
    Console.WriteLine("2. OneDrive");
    Console.Write("Ваш выбор: ");
    var locationChoice = Console.ReadLine();

    Console.WriteLine("\nВыберите формат файла:");
    Console.WriteLine("1. JSON");
    Console.WriteLine("2. XML");
    Console.Write("Ваш выбор: ");
    var formatChoice = Console.ReadLine();

    ISerializer<CourseWork.Models.Task> serializer = formatChoice switch
    {
        "1" => new JsonSerializer<CourseWork.Models.Task>(),
        "2" => new XmlSerializer<CourseWork.Models.Task>(),
        _ => null
    };

    if (serializer == null)
    {
        Console.WriteLine("Неверный выбор формата.");
        return;
    }

    Console.Write("Введите имя файла: ");
    var fileName = Console.ReadLine();
    var extension = formatChoice == "1" ? ".json" : ".xml";
    var fullFileName = fileName + extension;

    var storage = new StorageContext();
    storage.SetStrategy(locationChoice == "1"
        ? new LocalFileStorage()
        : new OneDriveLocalStorage());

    try
    {
        var tasks = taskManager.GetAllTasks().ToList();
        var content = serializer.SerializeToString(tasks);
        storage.Save(fullFileName, content);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
    }
}

static async System.Threading.Tasks.Task LoadFromFileAsync(ITaskManager taskManager)
{
    Console.WriteLine("\nВыберите место загрузки:");
    Console.WriteLine("1. Локально");
    Console.WriteLine("2. OneDrive");
    Console.Write("Ваш выбор: ");
    var locationChoice = Console.ReadLine();

    Console.WriteLine("\nВыберите формат файла:");
    Console.WriteLine("1. JSON");
    Console.WriteLine("2. XML");
    Console.Write("Ваш выбор: ");
    var formatChoice = Console.ReadLine();

    ISerializer<CourseWork.Models.Task> serializer = formatChoice switch
    {
        "1" => new JsonSerializer<CourseWork.Models.Task>(),
        "2" => new XmlSerializer<CourseWork.Models.Task>(),
        _ => null
    };

    if (serializer == null)
    {
        Console.WriteLine("Неверный выбор формата.");
        return;
    }

    Console.Write("Введите имя файла (с расширением): ");
    var fileName = Console.ReadLine();

    var storage = new StorageContext();
    storage.SetStrategy(locationChoice == "1"
        ? new LocalFileStorage()
        : new OneDriveLocalStorage());

    try
    {
        var content = storage.Load(fileName);
        if (string.IsNullOrEmpty(content))
        {
            Console.WriteLine("Не удалось загрузить файл");
            return;
        }

        var tasks = serializer.DeserializeFromString(content)?.ToList() ?? new List<CourseWork.Models.Task>();

        Console.WriteLine("Как загрузить задачи?");
        Console.WriteLine("1. Заменить текущие задачи");
        Console.WriteLine("2. Добавить к существующим");
        Console.Write("Ваш выбор: ");
        var loadChoice = Console.ReadLine();

        if (loadChoice == "1" && taskManager is TaskManager concreteManager)
        {
            concreteManager.ClearAllTasks();
        }

        foreach (var task in tasks)
        {
            if (task is Exam exam)
                taskManager.AddExam(exam);
            else
                taskManager.AddTask(task);
        }

        Console.WriteLine($"Успешно загружено {tasks.Count} задач");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
    }
}*/

static async System.Threading.Tasks.Task SaveToFileAsync(ITaskManager taskManager, ISerializer<CourseWork.Models.Task> serializer)
{
    Console.WriteLine("\nВыберите место сохранения:");
    Console.WriteLine("1. Локально");
    Console.WriteLine("2. OneDrive");
    Console.Write("Ваш выбор: ");
    var locationChoice = Console.ReadLine();

    Console.Write("Введите имя файла: ");
    var fileName = Console.ReadLine();

    var storage = new StorageContext();
    storage.SetStrategy(locationChoice == "1"
        ? new LocalFileStorage()
        : new OneDriveLocalStorage());

    try
    {
        var tasks = taskManager.GetAllTasks().ToList();
        var content = serializer.SerializeToString(tasks);
        storage.Save(fileName, content);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
    }
}

static async System.Threading.Tasks.Task LoadFromFileAsync(ITaskManager taskManager, ISerializer<CourseWork.Models.Task> serializer)
{
    Console.WriteLine("\nВыберите место загрузки:");
    Console.WriteLine("1. Локально");
    Console.WriteLine("2. OneDrive");
    Console.Write("Ваш выбор: ");
    var locationChoice = Console.ReadLine();

    Console.Write("Введите имя файла: ");
    var fileName = Console.ReadLine();

    var storage = new StorageContext();
    storage.SetStrategy(locationChoice == "1"
        ? new LocalFileStorage()
        : new OneDriveLocalStorage());

    try
    {
        var content = storage.Load(fileName);
        if (string.IsNullOrEmpty(content))
        {
            Console.WriteLine("Не удалось загрузить файл");
            return;
        }

        var tasks = serializer.DeserializeFromString(content);

        Console.WriteLine("Как загрузить задачи?");
        Console.WriteLine("1. Заменить текущие задачи");
        Console.WriteLine("2. Добавить к существующим");
        Console.Write("Ваш выбор: ");
        var loadChoice = Console.ReadLine();

        if (loadChoice == "1" && taskManager is TaskManager concreteManager)
        {
            concreteManager.ClearAllTasks();
        }

        foreach (var task in tasks)
        {
            if (task is Exam exam)
                taskManager.AddExam(exam);
            else
                taskManager.AddTask(task);
        }

        Console.WriteLine($"Успешно загружено {tasks.Count} задач");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
    }
}
void ShowAllTasks(ITaskManager taskManager)
{
    var tasks = taskManager.GetAllTasks();

    Console.WriteLine("\nВсе задания:");
    Console.WriteLine("=================================");

    if (tasks.Any())
    {
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
    }
    else
    {
        Console.WriteLine("Заданий не найдено.");
    }
}

void ShowAllExams(ITaskManager taskManager)
{
    var exams = taskManager.GetAllExams();

    Console.WriteLine("\nВсе экзамены:");
    Console.WriteLine("=================================");

    if (exams.Any())
    {
        foreach (var exam in exams)
        {
            Console.WriteLine(exam);
        }
    }
    else
    {
        Console.WriteLine("Экзаменов не найдено.");
    }
}

void MarkTaskCompleted(ITaskManager taskManager)
{
    ShowAllTasks(taskManager);

    Console.Write("\nВведите ID задания для отметки о выполнении: ");
    var id = Guid.Parse(Console.ReadLine());

    taskManager.MarkTaskAsCompleted(id);
    Console.WriteLine("Задание отмечено как выполненное!");
}

void RemoveTask(ITaskManager taskManager)
{
    ShowAllTasks(taskManager);

    Console.Write("\nВведите ID задания для удаления: ");
    var id = Guid.Parse(Console.ReadLine());

    taskManager.RemoveTask(id);
    Console.WriteLine("Задание удалено!");
}