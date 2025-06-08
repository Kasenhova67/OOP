

using System.Xml.Serialization;

namespace CourseWork.Models
{
    [Serializable]
    public class Exam : Task
    {
        public string ExamType { get; set; }
        public int PreparationTime { get; set; }
        public string Location { get; set; }

        [XmlIgnore]
        public bool IsResit { get; private set; }

        public Exam() : base()
        {
            ExamType = string.Empty;
            Location = string.Empty;
        }

        public Exam(string title, string description, DateTime dueDate,
                   string examType, int preparationTime, string location,
                   bool relatedToLesson = false, string lessonTitle = "")
            : base(title, description, dueDate, relatedToLesson, lessonTitle)
        {
            ExamType = examType;
            PreparationTime = preparationTime;
            Location = location;
            IsResit = false;
        }

        public void MarkAsResit()
        {
            IsResit = true;
        }


        public DateTime CalculatePreparationEndTime()
        {
            return DueDate.AddHours(-PreparationTime);
        }

        public override string ToString()
        {
            return base.ToString() +
                   $"\n  Exam Type: {ExamType} | " +
                   $"Location: {Location} | " +
                   $"Preparation Time: {PreparationTime} hours | " +
                   $"{(IsResit ? "Resit" : "Regular")}";
        }
    }
}
