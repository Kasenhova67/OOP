using Lab3OOp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Application.Services
{
    public interface IQuoteService
    {
        Task<QuoteDTO> GetMotivationalQuote();
    }
}
