using Lab3OOp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Application.Commands
{
    public class ViewStudentsCommand : ICommand
    {
        private readonly IStudentService _studentService;

        public ViewStudentsCommand(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public void Execute()
        {
            try
            {
                var students = _studentService.GetAllStudents();
                if (students == null || !students.Any())
                {
                    System.Console.WriteLine("No students found.");
                    return;
                }

                System.Console.WriteLine("\nStudent List:");
                System.Console.WriteLine("ID\tName\t\tGrade");
                System.Console.WriteLine("--------------------------------");
                foreach (var student in students)
                {
                    System.Console.WriteLine($"{student.Id}\t{student.Name}\t\t{student.Grade}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
