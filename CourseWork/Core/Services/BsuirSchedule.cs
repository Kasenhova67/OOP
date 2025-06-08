using CourseWork.Models;
using CourseWork.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace CourseWork.Services
{
    public class BSUIRScheduleService : IBSUIRScheduleService
    {
        private readonly HttpClient _httpClient;
       
            public BSUIRScheduleService(HttpClient httpClient)
            {
                _httpClient = httpClient;
                _httpClient.BaseAddress = new Uri("https://iis.bsuir.by/api/v1/");
            }

            public async Task<List<Exam>> GetExamsForGroupAsync(string groupId)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"schedule?studentGroup={groupId}");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var scheduleData = JsonConvert.DeserializeObject<ScheduleResponse>(content);

                    return ConvertToExams(scheduleData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при получении экзаменов: {ex.Message}");
                    return new List<Exam>();
                }
            }

            private List<Exam> ConvertToExams(ScheduleResponse scheduleData)
            {
                var exams = new List<Exam>();

                if (scheduleData.Exams != null)
                {
                    foreach (var examItem in scheduleData.Exams)
                    {
                        if (!string.IsNullOrEmpty(examItem.DateLesson))
                        {
                            try
                            {
                                var examDate = DateTime.ParseExact(examItem.DateLesson, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                                var startTime = ParseExamTime(examItem.StartLessonTime);
                                var endTime = ParseExamTime(examItem.EndLessonTime);

                                exams.Add(new Exam(
                                    title: examItem.SubjectFullName ?? examItem.Subject,
                                    description: $"Экзамен по {examItem.SubjectFullName ?? examItem.Subject}",
                                    dueDate: examDate.Date.Add(startTime), 
                                    examType: examItem.LessonTypeAbbrev == "Экзамен" ? "Экзамен" : "Зачет",
                                    preparationTime: 24, 
                                    location: examItem.Auditories?.FirstOrDefault() ?? "Аудитория не указана",
                                    relatedToLesson: true,
                                    lessonTitle: examItem.SubjectFullName ?? examItem.Subject
                                ));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка обработки экзамена: {ex.Message}");
                            }
                        }
                    }
                }

                return exams;
            }

            private TimeSpan ParseExamTime(string timeStr)
            {
                if (string.IsNullOrEmpty(timeStr)) return new TimeSpan(9, 0, 0); // По умолчанию 9:00

                var parts = timeStr.Split(':');
                if (parts.Length >= 2 && int.TryParse(parts[0], out var hours) &&
                                       int.TryParse(parts[1], out var minutes))
                {
                    return new TimeSpan(hours, minutes, 0);
                }

                return new TimeSpan(9, 0, 0);
            }
        
      
        public async Task<List<Lesson>> GetScheduleForGroupAsync(string groupId, DateTime date)
        {
            try
            {
                Console.WriteLine($"Запрос расписания для группы {groupId} на {date:dd.MM.yyyy}");

                var response = await _httpClient.GetAsync($"schedule?studentGroup={groupId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Получен ответ: {content.Substring(0, Math.Min(200, content.Length))}...");

                var scheduleData = JsonConvert.DeserializeObject<ScheduleResponse>(content);
                Console.WriteLine($"Десериализовано: {scheduleData.Schedules?.Count} дней расписания");

                var startDate = DateTime.ParseExact(scheduleData.StartDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(scheduleData.EndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                if (date.Date < startDate || date.Date > endDate)
                {
                    Console.WriteLine($"Внимание: Дата {date:dd.MM.yyyy} вне периода расписания ({scheduleData.StartDate}-{scheduleData.EndDate})");
                    return new List<Lesson>();
                }

                var lessons = ConvertToLessons(scheduleData, date);
                Console.WriteLine($"Найдено {lessons.Count} занятий");

                return lessons;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении расписания: {ex}");
                return new List<Lesson>();
            }
        }
      
        private List<Lesson> ConvertToLessons(ScheduleResponse scheduleData, DateTime date)
        {
            var lessons = new List<Lesson>();

            var startDate = DateTime.ParseExact(scheduleData.StartDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(scheduleData.EndDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);

            if (date.Date < startDate.Date || date.Date > endDate.Date)
            {
                Console.WriteLine($"Дата {date:dd.MM.yyyy} не входит в период расписания ({scheduleData.StartDate}-{scheduleData.EndDate})");
                return lessons;
            }

            var russianDayName = GetRussianDayName(date.DayOfWeek);

            if (scheduleData.Schedules != null &&
                scheduleData.Schedules.TryGetValue(russianDayName, out var daySchedule))
            {
                foreach (var item in daySchedule)
                {
                    if (IsLessonOnDate(item, date))
                    {
                        lessons.Add(new Lesson(
                            subject: item.SubjectFullName ?? item.Subject,
                            startTime: item.StartLessonTime,
                            endTime: item.EndLessonTime
                        ));
                    }
                }
            }

            return lessons;
        }

        private bool IsLessonOnDate(ScheduleItem item, DateTime date)
        {
            
            if (!string.IsNullOrEmpty(item.DateLesson))
            {
                var lessonDate = DateTime.ParseExact(item.DateLesson, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                return lessonDate.Date == date.Date;
            }

            if (!string.IsNullOrEmpty(item.StartLessonDate) && !string.IsNullOrEmpty(item.EndLessonDate))
            {
                var startDate = DateTime.ParseExact(item.StartLessonDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(item.EndLessonDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                if (date.Date < startDate.Date || date.Date > endDate.Date)
                    return false;
            }

            if (item.WeekNumber != null && item.WeekNumber.Count > 0)
            {
              
            }

            return true;
        }

        private string GetRussianDayName(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Понедельник",
                DayOfWeek.Tuesday => "Вторник",
                DayOfWeek.Wednesday => "Среда",
                DayOfWeek.Thursday => "Четверг",
                DayOfWeek.Friday => "Пятница",
                DayOfWeek.Saturday => "Суббота",
                DayOfWeek.Sunday => "Воскресенье",
                _ => string.Empty
            };
        }
    }
}