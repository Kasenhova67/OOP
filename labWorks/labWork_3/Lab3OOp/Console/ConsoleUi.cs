using Lab3OOp.Application.Services;
using Lab3OOp.Domain.DTOs;
using Lab3OOp.Application.Commands;
using System;


    namespace Lab3OOp.Console
{
        public class ConsoleUI
        {
            private readonly IStudentService _studentService;
            private readonly IQuoteService _quoteService;

            public ConsoleUI(IStudentService studentService, IQuoteService quoteService)
            {
                _studentService = studentService;
                _quoteService = quoteService;
            }

            public void Run()
            {
                while (true)
            {
                Thread.Sleep(1500);

                System.Console.WriteLine("\nStudent Record Management System");
                    System.Console.WriteLine("1. Add Student");
                    System.Console.WriteLine("2. Edit Student");
                    System.Console.WriteLine("3. View Students");
                    System.Console.WriteLine("4. Exit");
                    System.Console.Write("Enter your choice: ");

                    var choice = System.Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddStudent();
                            break;
                        case "2":
                            EditStudent();
                            break;
                        case "3":
                            ViewStudents();
                            break;
                        case "4":
                            return;
                        default:
                            System.Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }

            private void AddStudent()
            {
                System.Console.WriteLine("\nAdd New Student");
                var studentDto = GetStudentInput();
                if (studentDto != null)
                {
                    var command = new AddStudentCommand(_studentService, _quoteService, studentDto);
                    command.Execute();
                }
            }

            private void EditStudent()
            {
                System.Console.WriteLine("\nEdit Student");
                System.Console.Write("Enter Student ID to edit: ");
                if (!int.TryParse(System.Console.ReadLine(), out int id))
                {
                    System.Console.WriteLine("Invalid ID format.");
                    return;
                }

                var studentDto = GetStudentInput();
                if (studentDto != null)
                {
                    var command = new EditStudentCommand(_studentService, id, studentDto);
                    command.Execute();
                }
            }

            private void ViewStudents()
            {
                var command = new ViewStudentsCommand(_studentService);
                command.Execute();
            }

            private StudentDTO GetStudentInput()
            {
                System.Console.Write("Enter student name: ");
                var name = System.Console.ReadLine();

                System.Console.Write("Enter student grade (0-100): ");
                if (!int.TryParse(System.Console.ReadLine(), out int grade))
                {
                    System.Console.WriteLine("Invalid grade format. Please enter a number.");
                    return null;
                }

                return new StudentDTO { Name = name, Grade = grade };
            }
        }
    }
