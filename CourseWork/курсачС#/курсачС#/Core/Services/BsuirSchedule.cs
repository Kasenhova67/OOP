<<<<<<< HEAD
﻿
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
=======
﻿
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
            return new List<Lesson>
            {
                new Lesson("Programming", "09:00", "10:30"),
                new Lesson("Mathematics", "10:40", "12:10"),
                new Lesson("Physics", "13:00", "14:30")
            };
        }
    }
}
>>>>>>> 1db942a9e8ab65cb57a9decb16eea9788ae7a2f4
