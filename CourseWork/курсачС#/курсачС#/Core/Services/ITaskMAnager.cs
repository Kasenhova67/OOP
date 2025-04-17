using Models;
using System;
using System.Collections.Generic;
using Models;

namespace Services
{
    public interface ITaskManager
    {
        void AddTask(Models.Task task);
        void AddExam(Exam exam);
        IEnumerable<Models.Task> GetAllTasks();
        IEnumerable<Exam> GetAllExams();
        Models.Task GetTaskById(Guid id);
        void MarkTaskAsCompleted(Guid id);
        void RemoveTask(Guid id);
    }
}