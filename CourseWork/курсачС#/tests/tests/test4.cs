using CourseWork.Services;
using Xunit;

public class ScheduleServiceTests
{
    [Fact]
    public void GetScheduleForGroup_ReturnsMockLessons()
    {
       
        var service = new BSUIRScheduleService();

        
        var lessons = service.GetScheduleForGroup("12345", DateTime.Now);

        Assert.Equal(3, lessons.Count);
        Assert.Contains(lessons, l => l.Subject == "Programming");
    }
}