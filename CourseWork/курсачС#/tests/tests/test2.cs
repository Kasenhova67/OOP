using CourseWork.Models;
using Xunit;

public class ExamTests
{
    [Fact]
    public void Exam_Constructor_SetsPropertiesCorrectly()
    {
     
        var exam = new Exam("Math", "Final Exam", DateTime.Now.AddDays(7),
                        "written", 10, "Room 101");

        Assert.Equal("Math", exam.Title);
        Assert.Equal("written", exam.ExamType);
        Assert.Equal(10, exam.PreparationTime);
        Assert.False(exam.IsResit);
    }

    [Fact]
    public void MarkAsResit_SetsIsResitToTrue()
    {
        var exam = new Exam("Physics", "Midterm", DateTime.Now.AddDays(5),
                          "oral", 5, "Room 202");

        exam.MarkAsResit();

        Assert.True(exam.IsResit);
    }

    [Fact]
    public void CalculatePreparationEndTime_ReturnsCorrectDate()
    {
        var dueDate = DateTime.Now.AddDays(10);
        var exam = new Exam("Chemistry", "Lab Exam", dueDate, "practical", 24, "Lab");

        var prepEndTime = exam.CalculatePreparationEndTime();

        Assert.Equal(dueDate.AddHours(-24), prepEndTime);
    }
}