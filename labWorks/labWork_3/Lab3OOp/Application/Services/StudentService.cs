using Lab3OOp.Domain.DTOs;
using Lab3OOp.Domain.Entities;
using Lab3OOp.Domain.Validators;
using Lab3OOp.DataAccess.Repositories;
using Lab3OOp.Infrastrucrura.Factories;
using FluentValidation;


namespace Lab3OOp.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;
        private readonly IStudentFactory _studentFactory;
        private readonly IValidator<StudentDTO> _validator;

        public StudentService(IStudentRepository repository,
                            IStudentFactory studentFactory,
                            IValidator<StudentDTO> validator)
        {
            _repository = repository;
            _studentFactory = studentFactory;
            _validator = validator;
        }

        public void AddStudent(StudentDTO studentDto)
        {
            var validationResult = _validator.Validate(studentDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var student = _studentFactory.CreateStudent(studentDto);
            _repository.AddStudent(student);
            _repository.SaveChanges();
        }

        public void UpdateStudent(int id, StudentDTO studentDto)
        {
            var validationResult = _validator.Validate(studentDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingStudent = _repository.GetStudentById(id);
            if (existingStudent == null)
                throw new KeyNotFoundException("Student not found");

            var updatedStudent = _studentFactory.CreateStudent(studentDto);
            updatedStudent.Id = id;
            _repository.UpdateStudent(updatedStudent);
            _repository.SaveChanges();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _repository.GetAllStudents();
        }

        public Student GetStudentById(int id)
        {
            return _repository.GetStudentById(id);
        }
    }

}
