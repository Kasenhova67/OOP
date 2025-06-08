using CourseWork.Models;
using CourseWork.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CourseWork.Tests.Services
{
    public class BSUIRScheduleServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly BSUIRScheduleService _service;

        public BSUIRScheduleServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://iis.bsuir.by/api/v1/")
            };
            _service = new BSUIRScheduleService(_httpClient);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetExamsForGroupAsync_ReturnsExams_WhenApiReturnsData()
        {
            var response = new ScheduleResponse
            {
                Exams = new List<ExamItem>
                {
                    new ExamItem
                    {
                        Subject = "Math",
                        SubjectFullName = "Advanced Mathematics",
                        DateLesson = "01.01.2023",
                        StartLessonTime = "09:00",
                        EndLessonTime = "10:30",
                        LessonTypeAbbrev = "Экзамен",
                        Auditories = new List<string> { "101" }
                    }
                }
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            var result = await _service.GetExamsForGroupAsync("123456");

            Assert.Single(result);
            Assert.Equal("Advanced Mathematics", result[0].Title);
            Assert.Equal(new DateTime(2023, 1, 1, 9, 0, 0), result[0].DueDate);
            Assert.Equal("Экзамен", result[0].ExamType);
            Assert.Equal("101", result[0].Location);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetScheduleForGroupAsync_ReturnsLessons_WhenApiReturnsData()
        {
            var response = new ScheduleResponse
            {
                StartDate = "01.01.2023",
                EndDate = "31.12.2023",
                Schedules = new Dictionary<string, List<ScheduleItem>>
                {
                    ["Понедельник"] = new List<ScheduleItem>
                    {
                        new ScheduleItem
                        {
                            Subject = "Math",
                            SubjectFullName = "Advanced Mathematics",
                            StartLessonTime = "09:00",
                            EndLessonTime = "10:30",
                            Auditories = new List<string> { "101" }
                        }
                    }
                }
            };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(response))
                });

            
            var result = await _service.GetScheduleForGroupAsync("123456", new DateTime(2023, 1, 2)); 

            Assert.Single(result);
            Assert.Equal("Advanced Mathematics", result[0].Subject);
            Assert.Equal("09:00", result[0].StartTime);
            Assert.Equal("10:30", result[0].EndTime);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetExamsForGroupAsync_ReturnsEmptyList_WhenApiFails()
        {
            
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            
            var result = await _service.GetExamsForGroupAsync("123456");

            Assert.Empty(result);
        }
    }
}