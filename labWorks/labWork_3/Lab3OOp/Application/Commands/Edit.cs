using Lab3OOp.Application.Services;
using Lab3OOp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Application.Commands
{
    public class EditStudentCommand : ICommand
    {
        private readonly IStudentService _studentService;
        private readonly int _id;
        private readonly StudentDTO _studentDto;

        public EditStudentCommand(IStudentService studentService, int id, StudentDTO studentDto)
        {
            _studentService = studentService;
            _id = id;
            _studentDto = studentDto;
        }

        public void Execute()
        {
            try
            {
                _studentService.UpdateStudent(_id, _studentDto);
                System.Console.WriteLine("Student updated successfully!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
