using Dci.Mnm.Mwa.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dci.Mnm.Mwa.Infrastructure.Data.Mappings
{
    public class EmailAttachmentMapping : IEntityTypeConfiguration<EmailAttachment>
    {
        public void Configure(EntityTypeBuilder<EmailAttachment> builder)
        {
            builder.Ignore(x => x.Content);
        }
    }
}









