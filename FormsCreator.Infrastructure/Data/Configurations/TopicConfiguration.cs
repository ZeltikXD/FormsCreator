using FormsCreator.Core.Models;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class TopicConfiguration : EntityBaseConfiguration<Topic>
    {
        public override void Configure(EntityTypeBuilder<Topic> builder)
        {
            base.Configure(builder);

            builder.ToTable("Topics");

            builder.HasIndex(x => x.Name).IsUnique()
                .UseCollation("en_us_ci");

            builder.Property(x => x.Name).HasMaxLength(32);

            builder.HasData([new() { Id = Guid.NewGuid(), Name = "Education", CreatedAt = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Quiz", CreatedAt = DateTimeOffset.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Other", CreatedAt = DateTimeOffset.UtcNow }]);
        }
    }
}
