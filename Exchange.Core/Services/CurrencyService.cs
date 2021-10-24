using AutoMapper;
using Exchange.Data;
using Exchange.Items.Entities;
using Exchange.Items.Exceptions;
using Exchange.Items.Models.Enum;
using Exchange.Items.Models.Response;
using Exchange.Items.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Exchange.Core.Services
{
    public class CurrencyService : ServiceBase, ICurrencyService
    {
        private readonly IConfiguration _configuration;

        private readonly ApplicationContext _dbContext;

        private readonly ILogger<CurrencyService> _logger;

        private readonly IMapper _mapper;

        public CurrencyService(IConfiguration configuration, ApplicationContext dbContext, ILogger<CurrencyService> logger, IMapper mapper)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SaveDailyRatesAsync()
        {
            var result = new List<DailyRate>();

            using (var client = new HttpClient())
            {
                var request = await client.GetAsync(_configuration["Tcmb:ApiUrl"]);

                if (request.IsSuccessStatusCode)
                {
                    using (var stream = await request.Content.ReadAsStreamAsync())
                    {
                        XDocument dailyRates = XDocument.Load(stream);

                        result = dailyRates.Descendants("Currency")
                                       .Where(x => Enum.GetNames(typeof(Codes)).ToList().Contains(x.Attribute("CurrencyCode").Value))
                                       .Select(x => new DailyRate()
                                       {
                                           Id = Guid.NewGuid(),
                                           CreatedOn = DateTime.Now,
                                           UpdatedOn = DateTime.Now,
                                           Code = x.Attribute("CurrencyCode").Value.ToString(),
                                           CurrencyName = x.Element("CurrencyName").Value.ToString(),
                                           Name = x.Element("Isim").Value.ToString(),
                                           Rate = Convert.ToDecimal(x.Element("ForexSelling").Value, new CultureInfo("en-US"))
                                       }).ToList();
                    }
                }
            }

            if (result.Count > 0)
            {
                await _dbContext.DailyRates.AddRangeAsync(result);

                await _dbContext.SaveChangesAsync();
            }

            _logger.LogInformation("Process done.");
        }

        public async Task<List<DailyRateDto>> GetAllCurrenciesAsync(GetAllCurrenciesQuery query)
        {
            var result = new List<DailyRateDto>();

            var dailyRates = await _dbContext.DailyRates.Select(x => x.Code)
                                                        .Distinct()
                                                        .SelectMany(x => _dbContext.DailyRates.Where(c => c.Code == x).OrderByDescending(c => c.CreatedOn).Take(1))
                                                        .Select(group => new DailyRate()
                                                        {
                                                            Code = group.Code,
                                                            CreatedOn = group.CreatedOn,
                                                            CurrencyName = group.CurrencyName,
                                                            Id = group.Id,
                                                            Name = group.Name,
                                                            Rate = group.Rate,
                                                            UpdatedOn = group.UpdatedOn
                                                        }).ToListAsync();

            if (dailyRates.Count == 0)
            {
                throw new BusinessException("System does not have any rates.");
            }

            result = _mapper.Map<List<DailyRateDto>>(dailyRates);

            return SortingList(result, query.SortingField, query.SortingAsc);
        }

        public async Task<List<CurrencyRateDto>> GetCurrencyRatesAsync(GetCurrencyRatesQuery query)
        {
            var rates = await _dbContext.DailyRates.Where(x => x.Code == query.Code.ToUpper())
                                                   .Select(x => x.CreatedOn.Date)
                                                   .Distinct()
                                                   .SelectMany(x => _dbContext.DailyRates.Where(c => c.Code == query.Code.ToUpper() && c.CreatedOn.Date == x).OrderByDescending(c => c.CreatedOn).Take(1))
                                                   .Select(group => new DailyRate()
                                                   {
                                                       Code = group.Code,
                                                       CreatedOn = group.CreatedOn,
                                                       CurrencyName = group.CurrencyName,
                                                       Id = group.Id,
                                                       Name = group.Name,
                                                       Rate = group.Rate,
                                                       UpdatedOn = group.UpdatedOn
                                                   }).ToListAsync();

            if (rates.Count == 0)
            {
                throw new BusinessException($"System does not have any rates for {query.Code.ToUpper()}");
            }

            return _mapper.Map<List<CurrencyRateDto>>(rates).OrderBy(x => x.Date).ToList();
        }


        private List<DailyRateDto> SortingList(List<DailyRateDto> list, string sortingField, bool sortingAsc)
        {
            if (string.IsNullOrEmpty(sortingField) || sortingField?.ToLower() != ExchangeConstants.SortingField)
            {
                list = !sortingAsc ? list.OrderByDescending(x => x.Code).ToList() : list.OrderBy(x => x.Code).ToList();
            }
            else
            {
                list = !sortingAsc ? list.OrderByDescending(x => x.Rate).ToList() : list.OrderBy(x => x.Rate).ToList();
            }

            return list;
        }
    }
}
