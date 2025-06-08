using CourseWork.Models;
using CourseWork.Services;
using CourseWork.Serialization;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CourseWork.Tests.Services
{
    public class TaskManagerTests
    {
        private readonly Mock<ISerializer<CourseWork.Models.Task>> _serializerMock;
        private readonly TaskManager _taskManager;

        public TaskManagerTests()
        {
            _serializerMock = new Mock<ISerializer<CourseWork.Models.Task>>();
            _serializerMock.Setup(x => x.Deserialize(It.IsAny<string>())).Returns(new List<CourseWork.Models.Task>());
            _taskManager = new TaskManager(_serializerMock.Object, "test.json");
        }

        [Fact]
        public void AddTask_AddsTaskToCollection()
        {
           
            var task = new CourseWork.Models.Task("Test", "Test", DateTime.Now);

            _taskManager.AddTask(task);

            Assert.Single(_taskManager.GetAllTasks());
            _serializerMock.Verify(x => x.Serialize(It.IsAny<List<CourseWork.Models.Task>>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void AddExam_AddsExamToCollection()
        {
            var exam = new Exam("Test", "Test", DateTime.Now, "Exam", 10, "Room");

            _taskManager.AddExam(exam);

            Assert.Single(_taskManager.GetAllTasks());
            Assert.Single(_taskManager.GetAllExams());
            _serializerMock.Verify(x => x.Serialize(It.IsAny<List<CourseWork.Models.Task>>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetTaskById_ReturnsCorrectTask()
        {
            var task = new CourseWork.Models.Task("Test", "Test", DateTime.Now);
            _taskManager.AddTask(task);

            var result = _taskManager.GetTaskById(task.Id);

            Assert.Equal(task.Id, result.Id);
        }

        [Fact]
        public void MarkTaskAsCompleted_MarksTaskCompleted()
        {
           
            var task = new CourseWork.Models.Task("Test", "Test", DateTime.Now);
            _taskManager.AddTask(task);

            _taskManager.MarkTaskAsCompleted(task.Id);

            Assert.True(task.Completed);
            _serializerMock.Verify(x => x.Serialize(It.IsAny<List<CourseWork.Models.Task>>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void RemoveTask_RemovesTaskFromCollection()
        {
            
            var task = new CourseWork.Models.Task("Test", "Test", DateTime.Now);
            _taskManager.AddTask(task);

            _taskManager.RemoveTask(task.Id);

            Assert.Empty(_taskManager.GetAllTasks());
            _serializerMock.Verify(x => x.Serialize(It.IsAny<List<CourseWork.Models.Task>>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void ClearAllTasks_RemovesAllTasks()
        {
           
            _taskManager.AddTask(new CourseWork.Models.Task("Test1", "Test", DateTime.Now));
            _taskManager.AddTask(new CourseWork.Models.Task("Test2", "Test", DateTime.Now));

            _taskManager.ClearAllTasks();

            Assert.Empty(_taskManager.GetAllTasks());
            _serializerMock.Verify(x => x.Serialize(It.IsAny<List<CourseWork.Models.Task>>(), It.IsAny<string>()), Times.Exactly(3));
        }
    }
}