using Lab3OOp.Domain.DTOs;
using Lab3OOp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Infrastrucrura.Factories
{
    public class StudentFactory : IStudentFactory
    {
        public Student CreateStudent(StudentDTO studentDto)
        {
            return new Student
            {
                Name = studentDto.Name,
                Grade = studentDto.Grade
            };
        }
    }
}
