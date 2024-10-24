using FormsCreator.Core.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations.Base
{
    internal abstract class EntityUpdateConfiguration<TEntity> : EntityBaseConfiguration<TEntity> where TEntity : EntityUpdate
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("NOW()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
        }
    }
}
