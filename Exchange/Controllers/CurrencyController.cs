using Exchange.Core.Services;
using Exchange.Items.Models.Response;
using Exchange.Items.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exchange.Controllers
{
    [Route("api/exchange/v1/currencies")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DailyRateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrencies(GetAllCurrenciesQuery query)
        {
            return Ok(await _currencyService.GetAllCurrenciesAsync(query));
        }

        [HttpGet]
        [Route("{code}")]
        [ProducesResponseType(typeof(List<CurrencyRateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrencyRates([FromRoute] string code)
        {
            GetCurrencyRatesQuery query = new GetCurrencyRatesQuery()
            {
                Code = code
            };

            return Ok(await _currencyService.GetCurrencyRatesAsync(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrencies()
        {
            await _currencyService.SaveDailyRatesAsync();

            return Ok();
        }
    }
}
