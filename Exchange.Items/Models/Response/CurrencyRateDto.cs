using System;

namespace Exchange.Items.Models.Response
{
    public class CurrencyRateDto
    {
        public string Code { get; set; }

        public decimal Rate { get; set; }

        public string Date { get; set; }
    }
}
