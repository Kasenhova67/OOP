using System;
using Microsoft.Extensions.DependencyInjection;
using Lab3OOp.Infrastrucrura.Factories;
using Lab3OOp.DataAccess.Repositories;
using Lab3OOp.Infrastrucrura.Adapters;
using FluentValidation;
using Lab3OOp.Domain.DTOs;
using Lab3OOp.Domain.Validators;
using Lab3OOp.Application.Services;
using Lab3OOp.Console;

namespace Lab3OOp
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddHttpClient<IQuoteApiAdapter, QuoteApiAdapter>();
            services.AddSingleton<IStudentRepository, StudentRepository>();
            services.AddSingleton<IStudentFactory, StudentFactory>();
            services.AddSingleton<IQuoteFactory, QuoteFactory>();
            services.AddSingleton<IValidator<StudentDTO>, StudentValidator>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<IQuoteService, QuoteService>();
            services.AddSingleton<ConsoleUI>();

            var serviceProvider = services.BuildServiceProvider();
            var consoleUi = serviceProvider.GetService<ConsoleUI>();
            consoleUi.Run();
        }
    }
}
