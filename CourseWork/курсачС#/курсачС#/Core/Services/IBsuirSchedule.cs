
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
}