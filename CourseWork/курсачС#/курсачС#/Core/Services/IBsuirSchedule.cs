<<<<<<< HEAD
﻿
using CourseWork.Models;


namespace CourseWork.Services
{
    public interface IBSUIRScheduleService
    {
        List<Lesson> GetScheduleForGroup(string groupId, DateTime date);
    }
=======
﻿
using Models;
using System;
using System.Collections.Generic;
using Models;

namespace Services
{
    public interface IBSUIRScheduleService
    {
        List<Lesson> GetScheduleForGroup(string groupId, DateTime date);
    }
>>>>>>> 1db942a9e8ab65cb57a9decb16eea9788ae7a2f4
}