using Lab3OOp.Application.Services;
using Lab3OOp.Domain.DTOs;
using FluentValidation;


namespace Lab3OOp.Application.Commands
{
    public class AddStudentCommand : ICommand
    {
        private readonly IStudentService _studentService;
        private readonly IQuoteService _quoteService;
        private readonly StudentDTO _studentDto;

        public AddStudentCommand(IStudentService studentService, IQuoteService quoteService, StudentDTO studentDto)
        {
            _studentService = studentService;
            _quoteService = quoteService;
            _studentDto = studentDto;
        }

        public async void Execute()
        {
            try
            {
                _studentService.AddStudent(_studentDto);
                 System.Console.WriteLine("Student added successfully!");

                try
                {
                    var quote = await _quoteService.GetMotivationalQuote();
                    System.Console.WriteLine("\nMotivational Quote:");
                    System.Console.WriteLine($"\"{quote.Content}\" - {quote.Author}");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("\nCouldn't fetch a new quote, but here's one for you:");
                    System.Console.WriteLine("\"The expert in anything was once a beginner.\" - Helen Hayes");
                }
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    System.Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

