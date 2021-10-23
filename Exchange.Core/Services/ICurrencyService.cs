using Exchange.Items.Models.Response;
using Exchange.Items.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exchange.Core.Services
{
    public interface ICurrencyService : IServiceBase
    {
        Task SaveDailyRatesAsync();

        Task<List<DailyRateDto>> GetAllCurrenciesAsync(GetAllCurrenciesQuery query);

        Task<List<CurrencyRateDto>> GetCurrencyRatesAsync(GetCurrencyRatesQuery query);
    }
}
