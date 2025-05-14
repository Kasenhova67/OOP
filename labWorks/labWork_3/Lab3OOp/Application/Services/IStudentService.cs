using Lab3OOp.Domain.DTOs;
using Lab3OOp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Application.Services
{
    public interface IStudentService
    {
        void AddStudent(StudentDTO studentDto);
        void UpdateStudent(int id, StudentDTO studentDto);
        IEnumerable<Student> GetAllStudents();
        Student GetStudentById(int id);
    }
}
