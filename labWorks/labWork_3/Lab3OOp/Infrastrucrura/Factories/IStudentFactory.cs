using Lab3OOp.Domain.DTOs;
using Lab3OOp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Infrastrucrura.Factories
{
    public interface IStudentFactory
    {
        Student CreateStudent(StudentDTO studentDto);
    }
}
