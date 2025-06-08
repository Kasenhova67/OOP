
using CourseWork.Models;


namespace CourseWork.Services
{
    public interface IBSUIRScheduleService
    {
        Task<List<Exam>> GetExamsForGroupAsync(string groupId);
        Task<List<Lesson>> GetScheduleForGroupAsync(string groupId, DateTime date);
    }
}