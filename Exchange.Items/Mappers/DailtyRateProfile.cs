using AutoMapper;
using Exchange.Items.Entities;
using Exchange.Items.Models.Response;

namespace Exchange.Items.Mappers
{
    public class DailtyRateProfile : Profile
    {
        public DailtyRateProfile()
        {
            CreateMap<DailyRate, DailyRateDto>().AfterMap((src, dest) =>
            {
                dest.Code = $"TRY/{src.Code}";
                dest.Rate = src.Rate;
            });

            CreateMap<DailyRate, CurrencyRateDto>().AfterMap((src, dest) =>
            {
                dest.Code = $"TRY/{src.Code}";
                dest.Rate = src.Rate;
                dest.Date = src.CreatedOn.ToString("dd.MM.yyyy");
            });
        }
    }
}
