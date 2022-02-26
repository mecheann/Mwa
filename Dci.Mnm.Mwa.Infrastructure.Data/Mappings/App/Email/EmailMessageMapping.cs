using System.Collections.Generic;
using Dci.Mnm.Mwa.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dci.Mnm.Mwa.Infrastructure.Data.Mappings
{
    public class EmailMessageMapping : IEntityTypeConfiguration<EmailMessage>
    {
        public void Configure(EntityTypeBuilder<EmailMessage> builder)
        {
            var jsonConverter = new ValueConverter<List<EmailAddress>, string>(
          v => Newtonsoft.Json.JsonConvert.SerializeObject(v),
          v => Newtonsoft.Json.JsonConvert
                      .DeserializeObject<List<EmailAddress>>(v));

            builder.Property(x => x.FromAddresses).HasConversion(jsonConverter);
            builder.Property(x => x.ToAddresses).HasConversion(jsonConverter);
            builder.Property(x => x.ToCCAddresses).HasConversion(jsonConverter);
            builder.Property(x => x.ToBCCAddresses).HasConversion(jsonConverter);

            builder.Property(x => x.Content)
                .IsUnicode(false)
                .IsFixedLength(false)
                .HasMaxLength(16 * 1026 * 1026);
            builder.Property(x => x.Status).ValueGeneratedNever().HasDefaultValue(EmailStatus.Pending);
        }
    }
}









