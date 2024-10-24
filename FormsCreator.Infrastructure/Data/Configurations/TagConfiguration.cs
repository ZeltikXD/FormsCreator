using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal class TagConfiguration : EntityBaseConfiguration<Tag>
    {
        public override void Configure(EntityTypeBuilder<Tag> builder)
        {
            base.Configure(builder);

            builder.ToTable("Tags");

            builder.HasIndex(x => x.Name).IsUnique()
                .UseCollation("en_us_ci");

            builder.Property(x => x.Name).HasMaxLength(Constraints.MAX_LENGTH_TAG_NAME);

        }
    }
}
