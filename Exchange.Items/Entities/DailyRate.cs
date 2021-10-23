namespace Exchange.Items.Entities
{
    public class DailyRate : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string CurrencyName { get; set; }

        public decimal Rate { get; set; }
    }
}
