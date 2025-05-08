<<<<<<< HEAD
﻿
namespace CourseWork.Models
{
    public class Lesson
    {
        public string Subject { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public Lesson(string subject, string startTime, string endTime)
        {
            Subject = subject;
            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString()
        {
            return $"{Subject} ({StartTime} - {EndTime})";
        }
    }
=======
﻿
namespace Models
{
    public class Lesson
    {
        public string Subject { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public Lesson(string subject, string startTime, string endTime)
        {
            Subject = subject;
            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString()
        {
            return $"{Subject} ({StartTime} - {EndTime})";
        }
    }
>>>>>>> 1db942a9e8ab65cb57a9decb16eea9788ae7a2f4
}