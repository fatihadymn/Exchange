using Exchange.Items.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.Data.Configurations
{
    public class DailyRateConfiguration : IEntityTypeConfiguration<DailyRate>
    {
        public void Configure(EntityTypeBuilder<DailyRate> builder)
        {
            builder.ToTable("daily_rates");

            builder.Property(x => x.Id)
                   .HasColumnType("uuid")
                   .HasColumnName("id")
                   .IsRequired();

            builder.Property(x => x.CreatedOn)
                   .HasColumnName("created_on")
                   .HasColumnType("timestamp")
                   .IsRequired();

            builder.Property(x => x.UpdatedOn)
                   .HasColumnName("updated_on")
                   .HasColumnType("timestamp")
                   .IsRequired();

            builder.Property(x => x.Code)
                   .HasColumnType("varchar")
                   .HasColumnName("code")
                   .IsRequired();

            builder.Property(x => x.Name)
                   .HasColumnName("name")
                   .HasColumnType("varchar")
                   .IsRequired();

            builder.Property(x => x.CurrencyName)
                   .HasColumnType("varchar")
                   .HasColumnName("currency_name")
                   .IsRequired();

            builder.Property(x => x.Rate)
                   .HasColumnName("rate")
                   .HasColumnType("money")
                   .IsRequired();
        }
    }
}
