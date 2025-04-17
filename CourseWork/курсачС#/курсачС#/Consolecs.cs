using Task = Models.Task;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Globalization;


namespace Console
{
    public class ConsoleUI
    {
        private readonly ITaskManager _taskManager;
        private readonly IBSUIRScheduleService _scheduleService;

        public ConsoleUI(ITaskManager taskManager, IBSUIRScheduleService scheduleService)
        {
            _taskManager = taskManager;
            _scheduleService = scheduleService;
        }

        public void Run()
        {
            while (true)
            {
                System.Console.WriteLine("\nTask Manager Menu:");
                System.Console.WriteLine("1. Add Task");
                System.Console.WriteLine("2. Add Exam");
                System.Console.WriteLine("3. View All Tasks");
                System.Console.WriteLine("4. View All Exams");
                System.Console.WriteLine("5. Mark Task as Completed");
                System.Console.WriteLine("6. Remove Task");
                System.Console.WriteLine("7. Exit");
                System.Console.Write("Select an option: ");

                var input = System.Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        AddExam();
                        break;
                    case "3":
                        ListAllTasks();
                        break;
                    case "4":
                        ListAllExams();
                        break;
                    case "5":
                        MarkTaskAsCompleted();
                        break;
                    case "6":
                        RemoveTask();
                        break;
                    case "7":
                        return;
                    default:
                        System.Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void AddTask()
        {
            System.Console.Write("Enter task title: ");
            var title = System.Console.ReadLine();

            System.Console.Write("Enter task description: ");
            var description = System.Console.ReadLine();

            System.Console.Write("Is this task related to a lesson? (y/n): ");
            var relatedToLesson = System.Console.ReadLine()?.ToLower() == "y";

            DateTime dueDate;
            string lessonTitle = "";

            if (relatedToLesson)
            {
                System.Console.Write("Enter group number: ");
                var groupId = System.Console.ReadLine();

                System.Console.Write("Enter date (yyyy-MM-dd): ");
                var dateInput = System.Console.ReadLine();

                if (!DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    System.Console.WriteLine("Invalid date format. Using today's date.");
                    date = DateTime.Today;
                }

                var lessons = _scheduleService.GetScheduleForGroup(groupId, date);

                System.Console.WriteLine("Available lessons:");
                for (int i = 0; i < lessons.Count; i++)
                {
                    System.Console.WriteLine($"{i + 1}. {lessons[i]}");
                }

                System.Console.Write("Select a lesson (number): ");
                if (int.TryParse(System.Console.ReadLine(), out int lessonIndex) && lessonIndex > 0 && lessonIndex <= lessons.Count)
                {
                    var selectedLesson = lessons[lessonIndex - 1];
                    lessonTitle = selectedLesson.Subject;

                    // Parse time from lesson (simplified)
                    if (DateTime.TryParseExact($"{date:yyyy-MM-dd} {selectedLesson.StartTime}",
                        "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
                    {
                        System.Console.WriteLine($"Due date automatically set to: {dueDate}");
                    }
                    else
                    {
                        System.Console.WriteLine("Couldn't parse lesson time. Please enter due date manually.");
                        dueDate = GetDueDateFromUser();
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid lesson selection. Please enter due date manually.");
                    dueDate = GetDueDateFromUser();
                }
            }
            else
            {
                dueDate = GetDueDateFromUser();
            }

            var task = new Task(title, description, dueDate, relatedToLesson, lessonTitle);
            _taskManager.AddTask(task);
            System.Console.WriteLine("Task added successfully!");
        }

        private void AddExam()
        {
            System.Console.Write("Enter exam title: ");
            var title = System.Console.ReadLine();

            System.Console.Write("Enter exam description: ");
            var description = System.Console.ReadLine();

            var dueDate = GetDueDateFromUser();

            System.Console.Write("Enter exam type (e.g., written, oral, online): ");
            var examType = System.Console.ReadLine();

            System.Console.Write("Enter preparation time in hours: ");
            if (!int.TryParse(System.Console.ReadLine(), out int prepTime))
            {
                prepTime = 0;
            }

            System.Console.Write("Enter location: ");
            var location = System.Console.ReadLine();

            System.Console.Write("Is this a resit exam? (y/n): ");
            var isResit = System.Console.ReadLine()?.ToLower() == "y";

            System.Console.Write("Is this exam related to a lesson? (y/n): ");
            var relatedToLesson = System.Console.ReadLine()?.ToLower() == "y";
            string lessonTitle = "";

            if (relatedToLesson)
            {
                System.Console.Write("Enter related lesson title: ");
                lessonTitle = System.Console.ReadLine();
            }

            var exam = new Exam(title, description, dueDate, examType, prepTime, location, relatedToLesson, lessonTitle);

            if (isResit)
            {
                exam.MarkAsResit();
            }

            _taskManager.AddExam(exam);
            System.Console.WriteLine("Exam added successfully!");
        }

        private DateTime GetDueDateFromUser()
        {
            DateTime dueDate;
            while (true)
            {
                System.Console.Write("Enter due date (yyyy-MM-dd HH:mm): ");
                var dueDateInput = System.Console.ReadLine();

                if (DateTime.TryParseExact(dueDateInput, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
                {
                    break;
                }
                System.Console.WriteLine("Invalid date format. Please use yyyy-MM-dd HH:mm format.");
            }
            return dueDate;
        }

        private void ListAllTasks()
        {
            var tasks = _taskManager.GetAllTasks();
            if (!tasks.Any())
            {
                System.Console.WriteLine("No tasks found.");
                return;
            }

            System.Console.WriteLine("\nAll Tasks:");
            foreach (var task in tasks)
            {
                System.Console.WriteLine(task);
            }
        }

        private void ListAllExams()
        {
            var exams = _taskManager.GetAllExams();
            if (!exams.Any())
            {
                System.Console.WriteLine("No exams found.");
                return;
            }

            System.Console.WriteLine("\nAll Exams:");
            foreach (var exam in exams)
            {
                System.Console.WriteLine(exam);
                System.Console.WriteLine($"  Preparation should be done by: {exam.CalculatePreparationEndTime():yyyy-MM-dd HH:mm}");
            }
        }

        private void MarkTaskAsCompleted()
        {
            var taskId = GetTaskIdFromUser();
            if (taskId == Guid.Empty) return;

            _taskManager.MarkTaskAsCompleted(taskId);
            System.Console.WriteLine("Task marked as completed!");
        }

        private void RemoveTask()
        {
            var taskId = GetTaskIdFromUser();
            if (taskId == Guid.Empty) return;

            _taskManager.RemoveTask(taskId);
            System.Console.WriteLine("Task removed successfully!");
        }

        private Guid GetTaskIdFromUser()
        {
            System.Console.Write("Enter task ID: ");
            if (!Guid.TryParse(System.Console.ReadLine(), out var taskId))
            {
                System.Console.WriteLine("Invalid task ID format.");
                return Guid.Empty;
            }

            var task = _taskManager.GetTaskById(taskId);
            if (task == null)
            {
                System.Console.WriteLine("Task not found.");
                return Guid.Empty;
            }

            return taskId;
        }
    }
}