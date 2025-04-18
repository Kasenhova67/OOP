<<<<<<< HEAD
﻿using CourseWork.Models;

namespace CourseWork.Services
{
    public class TaskManager : ITaskManager
    {
        private readonly List<Models.Task> _tasks = new List<Models.Task>();

        public void AddTask(Models.Task task)
        {
            _tasks.Add(task);
        }

        public void AddExam(Exam exam)
        {
            _tasks.Add(exam);
        }

        public IEnumerable<Models.Task> GetAllTasks()
        {
            return _tasks;
        }

        public IEnumerable<Exam> GetAllExams()
        {
            return _tasks.OfType<Exam>();
        }

        public Models.Task GetTaskById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void MarkTaskAsCompleted(Guid id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.MarkAsCompleted();
            }
        }

        public void RemoveTask(Guid id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }
    }
=======
﻿using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Services
{
    public class TaskManager : ITaskManager
    {
        private readonly List<Models.Task> _tasks = new List<Models.Task>();

        public void AddTask(Models.Task task)
        {
            _tasks.Add(task);
        }

        public void AddExam(Exam exam)
        {
            _tasks.Add(exam);
        }

        public IEnumerable<Models.Task> GetAllTasks()
        {
            return _tasks;
        }

        public IEnumerable<Exam> GetAllExams()
        {
            return _tasks.OfType<Exam>();
        }

        public Models.Task GetTaskById(Guid id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }

        public void MarkTaskAsCompleted(Guid id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                task.MarkAsCompleted();
            }
        }

        public void RemoveTask(Guid id)
        {
            var task = GetTaskById(id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
        }
    }
>>>>>>> 1db942a9e8ab65cb57a9decb16eea9788ae7a2f4
}