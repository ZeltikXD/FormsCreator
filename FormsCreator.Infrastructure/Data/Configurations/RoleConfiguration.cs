using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class RoleConfiguration : EntityBaseConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder.ToTable("Roles");

            builder.HasIndex(x => x.Name).IsUnique().UseCollation("en_us_ci");

            builder.Property(x => x.Name).HasMaxLength(32);

            builder.HasData([new() { Id = Constants.UserRoleId, Name = "User", CreatedAt = DateTimeOffset.UtcNow },
            new() { Id = Constants.AdminRoleId, Name = "Admin", CreatedAt = DateTimeOffset.UtcNow }]);
        }
    }
}
