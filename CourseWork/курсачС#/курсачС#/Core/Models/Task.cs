<<<<<<< HEAD
﻿

namespace CourseWork.Models
{
    public class Task
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; private set; }
        public bool RelatedToLesson { get; set; }
        public string LessonTitle { get; set; }

        public Task(string title, string description, DateTime dueDate, bool relatedToLesson = false, string lessonTitle = "")
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DueDate = dueDate;
            Completed = false;
            RelatedToLesson = relatedToLesson;
            LessonTitle = lessonTitle;
        }

        public void MarkAsCompleted()
        {
            Completed = true;
        }

        public bool IsDue()
        {
            return DateTime.Now >= DueDate;
        }

        public override string ToString()
        {
            return $"[{Id}] {Title} - Due: {DueDate:yyyy-MM-dd HH:mm} | " +
                   $"{(Completed ? "Completed" : "Pending")} | " +
                   $"{(RelatedToLesson ? $"Related to lesson: {LessonTitle}" : "Not related to lesson")}";
        }
    }
=======
﻿

namespace Models
{
    public class Task
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; private set; }
        public bool RelatedToLesson { get; set; }
        public string LessonTitle { get; set; }

        public Task(string title, string description, DateTime dueDate, bool relatedToLesson = false, string lessonTitle = "")
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            DueDate = dueDate;
            Completed = false;
            RelatedToLesson = relatedToLesson;
            LessonTitle = lessonTitle;
        }

        public void MarkAsCompleted()
        {
            Completed = true;
        }

        public bool IsDue()
        {
            return DateTime.Now >= DueDate;
        }

        public override string ToString()
        {
            return $"[{Id}] {Title} - Due: {DueDate:yyyy-MM-dd HH:mm} | " +
                   $"{(Completed ? "Completed" : "Pending")} | " +
                   $"{(RelatedToLesson ? $"Related to lesson: {LessonTitle}" : "Not related to lesson")}";
        }
    }
>>>>>>> 1db942a9e8ab65cb57a9decb16eea9788ae7a2f4
}