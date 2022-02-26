using Dci.Mnm.Mwa.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dci.Mnm.Mwa.Infrastructure.Data.Mappings
{
    public class OperationMapping : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.HasKey(x=> x.Name);
        }
    }
}








