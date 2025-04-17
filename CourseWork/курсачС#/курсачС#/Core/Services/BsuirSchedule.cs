
using Models;
using Services;
using System;
using System.Collections.Generic;


namespace Services
{
    public class BSUIRScheduleService : IBSUIRScheduleService
    {
        // This is a mock implementation - in a real app, this would call the actual BSUIR API
        public List<Lesson> GetScheduleForGroup(string groupId, DateTime date)
        {
            // Mock data , will be  replaced with actual API call
            return new List<Lesson>
            {
                new Lesson("Programming", "09:00", "10:30"),
                new Lesson("Mathematics", "10:40", "12:10"),
                new Lesson("Physics", "13:00", "14:30")
            };
        }
    }
}