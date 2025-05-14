using Xunit;
using Moq;
using Lab3OOp.Application.Services;
using Lab3OOp.Domain.DTOs;
using Lab3OOp.Domain.Entities;
using Lab3OOp.DataAccess.Repositories;
using Lab3OOp.Infrastrucrura.Factories;
using FluentValidation;
using FluentValidation.TestHelper;
using Lab3OOp.Application.Commands;
using Lab3OOp.Application.Services;
using Lab3OOp.Infrastrucrura.Adapters;
using Moq.Protected;
using Newtonsoft.Json;
using Lab3OOp.Domain.Validators;

public class AddStudentTest
{
    [Fact]
    public void AddStudentCommand_ShouldAddStudentAndDisplayQuote()
    {
        // Arrange
        var studentDto = new StudentDTO { Name = "John Doe", Grade = 85 };

        var mockStudentService = new Mock<IStudentService>();
        var mockQuoteService = new Mock<IQuoteService>();

        mockQuoteService.Setup(q => q.GetMotivationalQuote())
            .ReturnsAsync(new QuoteDTO
            {
                Content = "Test quote",
                Author = "Test author"
            });

        var command = new AddStudentCommand(
            mockStudentService.Object,
            mockQuoteService.Object,
            studentDto);

        var consoleOutput = new StringWriter();
        System.Console.SetOut(consoleOutput);

        // Act
        command.Execute();

        // Assert
        mockStudentService.Verify(s => s.AddStudent(studentDto), Times.Once);
        mockQuoteService.Verify(q => q.GetMotivationalQuote(), Times.Once);

        var output = consoleOutput.ToString();
        Assert.Contains("Student added successfully!", output);
        Assert.Contains("Test quote", output);
        Assert.Contains("Test author", output);
    }
}

public class QuoteApiTest
{
    [Fact]
    public async Task QuoteApiAdapter_ShouldReturnProperlyParsedQuote()
    {
        // Arrange
        var expectedQuote = new QuoteDTO
        {
            Content = "Test content",
            Author = "Test author"
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedQuote))
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var adapter = new QuoteApiAdapter(httpClient);

        // Act
        var result = await adapter.GetMotivationalQuote();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedQuote.Content, result.Content);
        Assert.Equal(expectedQuote.Author, result.Author);
    }

    [Fact]
    public async Task QuoteService_ShouldReturnFallbackQuote_WhenApiFails()
    {
        // Arrange
        var mockAdapter = new Mock<IQuoteApiAdapter>();
        mockAdapter.Setup(a => a.GetMotivationalQuote())
            .ReturnsAsync((QuoteDTO)null);

        var mockFactory = new Mock<IQuoteFactory>();
        var service = new QuoteService(mockAdapter.Object, mockFactory.Object);

        // Act
        var result = await service.GetMotivationalQuote();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Success is not final, failure is not fatal: It is the courage to continue that counts.", result.Content);
        Assert.Equal("Winston Churchill", result.Author);
    }
}



public class StudentValidationTest
{
    private readonly StudentValidator _validator = new StudentValidator();

    [Theory]
    [InlineData("", false)] // Empty name
    [InlineData("A", false)] // Too short name
    [InlineData("Valid Name", true)] // Valid name
    public void ValidateStudentName(string name, bool shouldBeValid)
    {
        var student = new StudentDTO { Name = name, Grade = 50 };

        var result = _validator.TestValidate(student);

        if (shouldBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.Name);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(s => s.Name);
        }
    }

    [Theory]
    [InlineData(-1, false)] // Below minimum
    [InlineData(0, true)] // Minimum
    [InlineData(50, true)] // Valid
    [InlineData(100, true)] // Maximum
    [InlineData(101, false)] // Above maximum
    public void ValidateStudentGrade(int grade, bool shouldBeValid)
    {
        var student = new StudentDTO { Name = "Valid Name", Grade = grade };

        var result = _validator.TestValidate(student);

        if (shouldBeValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.Grade);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(s => s.Grade);
        }
    }

    
}