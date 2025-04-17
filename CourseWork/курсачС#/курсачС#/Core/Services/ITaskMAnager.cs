<<<<<<< HEAD
﻿using CourseWork.Models;


namespace CourseWork.Services
{
    public interface ITaskManager
    {
        void AddTask(CourseWork.Models.Task task);
        void AddExam(Exam exam);
        IEnumerable<CourseWork.Models.Task> GetAllTasks();
        IEnumerable<Exam> GetAllExams();
        CourseWork.Models.Task GetTaskById(Guid id);
        void MarkTaskAsCompleted(Guid id);
        void RemoveTask(Guid id);
    }
=======
﻿using Models;
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
>>>>>>> 1db942a9e8ab65cb57a9decb16eea9788ae7a2f4
}