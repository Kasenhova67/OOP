
using CourseWork.Models;

//пустышка 

namespace CourseWork.Services
{
    public class BSUIRScheduleService : IBSUIRScheduleService
    {
        public List<Lesson> GetScheduleForGroup(string groupId, DateTime date)
        {
           
            return new List<Lesson>
            {
                new Lesson("Programming", "09:00", "10:30"),
                new Lesson("Mathematics", "10:40", "12:10"),
                new Lesson("Physics", "13:00", "14:30")
            };
        }
    }
}