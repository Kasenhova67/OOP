using CourseWork.Models; 
using Xunit;
using System;

namespace TestProject1.ModelsTests 
{
    public class TaskTests
    {
        [Fact]
        public void MinimalTest()
        {
            var task = new CourseWork.Models.Task("Test", "Test", DateTime.Now);
            Assert.NotNull(task);
        }
    }
}