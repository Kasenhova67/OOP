using Lab3OOp.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.DataAccess.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _filePath = "students.json";
        private List<Student> _students;

        public StudentRepository()
        {
            LoadStudents();
        }

        public void AddStudent(Student student)
        {
            _students.Add(student);
            SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = _students.Find(s => s.Id == student.Id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Grade = student.Grade;
                SaveChanges();
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _students;
        }

        public Student GetStudentById(int id)
        {
            return _students.Find(s => s.Id == id);
        }

        public void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(_students, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        private void LoadStudents()
        {
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                _students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
            }
            else
            {
                _students = new List<Student>();
            }

             if (_students.Count > 0)
            {
                Student.NextId = _students.Max(s => s.Id) + 1;
            }
        }
    }
}
