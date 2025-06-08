using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Services
{
    public class ScheduleResponse
    {
        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("schedules")]
        public Dictionary<string, List<ScheduleItem>> Schedules { get; set; }

        public List<ExamItem> Exams { get; set; }
    }

    public class ScheduleItem
    {

        public List<string> Auditories { get; set; }
        public string EndLessonTime { get; set; }
        public string LessonTypeAbbrev { get; set; }
        public string Note { get; set; }
        public int NumSubgroup { get; set; }
        public string StartLessonTime { get; set; }
        public List<StudentGroup> StudentGroups { get; set; }
        public string Subject { get; set; }
        public string SubjectFullName { get; set; }
        public List<int> WeekNumber { get; set; }
        public List<Employee> Employees { get; set; }
        public string DateLesson { get; set; }
        public string StartLessonDate { get; set; }
        public string EndLessonDate { get; set; }
    }

    public class ExamItem : ScheduleItem
    {
        
    }

    public class StudentGroup
    {
        public string Name { get; set; }
        
    }

    public class Employee
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        
    }
}
