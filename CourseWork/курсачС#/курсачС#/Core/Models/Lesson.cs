
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
}