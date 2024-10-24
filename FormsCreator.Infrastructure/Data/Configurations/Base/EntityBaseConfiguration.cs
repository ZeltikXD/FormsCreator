using FormsCreator.Core.Models.Base;
using FormsCreator.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations.Base
{
    internal abstract class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();

            builder.Property(x => x.CreatedAt)
                .HasValueGenerator<UtcDateTimeValueGenerator>()
                .ValueGeneratedOnAdd();
        }
    }
}
