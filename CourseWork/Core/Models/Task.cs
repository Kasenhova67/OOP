

using System.Xml.Serialization;

namespace CourseWork.Models
{
    [Serializable]
    [XmlInclude(typeof(Exam))] 

    public class Task
    {
        public Guid Id { get;  set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        [XmlIgnore]
        public bool Completed { get; private set; }
        public bool RelatedToLesson { get; set; }
        public string LessonTitle { get; set; }

       
        public Task()
        {
            Id = Guid.NewGuid();
            Title = string.Empty;
            Description = string.Empty;
            DueDate = DateTime.Now;
        }

        public Task(string title, string description, DateTime dueDate,
                   bool relatedToLesson = false, string lessonTitle = "")
            : this() 
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
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
}