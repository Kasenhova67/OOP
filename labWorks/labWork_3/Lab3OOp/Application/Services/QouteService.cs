using Lab3OOp.Domain.DTOs;

using Lab3OOp.Infrastrucrura.Adapters;
using Lab3OOp.Infrastrucrura.Factories;
using System.Threading.Tasks;

namespace Lab3OOp.Application.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteApiAdapter _quoteApiAdapter;
        private readonly IQuoteFactory _quoteFactory;

        public QuoteService(IQuoteApiAdapter quoteApiAdapter, IQuoteFactory quoteFactory)
        {
            _quoteApiAdapter = quoteApiAdapter;
            _quoteFactory = quoteFactory;
        }

        public async Task<QuoteDTO> GetMotivationalQuote()
        {
            var quoteDto = await _quoteApiAdapter.GetMotivationalQuote();

            // Fallback if API call fails
            if (quoteDto == null)
            {
                return new QuoteDTO
                {
                    Content = "Success is not final, failure is not fatal: It is the courage to continue that counts.",
                    Author = "Winston Churchill"
                };
            }

            return quoteDto;
        }
    }
}
