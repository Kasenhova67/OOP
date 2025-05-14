using Lab3OOp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.DataAccess.Repositories
{
    public interface IStudentRepository
    {
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        IEnumerable<Student> GetAllStudents();
        Student GetStudentById(int id);
        void SaveChanges();
    }
}
