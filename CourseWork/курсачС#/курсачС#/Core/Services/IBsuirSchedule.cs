
using CourseWork.Models;


namespace CourseWork.Services
{
    public interface IBSUIRScheduleService
    {
        List<Lesson> GetScheduleForGroup(string groupId, DateTime date);
    }
}