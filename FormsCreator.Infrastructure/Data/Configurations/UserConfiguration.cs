using FormsCreator.Core.Enums;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class UserConfiguration : EntityBaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("Users");

            builder.Property(x => x.UserName)
                .HasMaxLength(Constraints.MAX_LENGTH_USERNAME);

            builder.HasIndex(x => x.UserName)
                .HasDatabaseName("Index_Users_UserName_Trigram")
                .HasMethod("GIN").HasOperators("gin_trgm_ops")
                .UseCollation("en_us_ci")
                .IsCreatedConcurrently();

            builder.Property(x => x.Email)
                .HasMaxLength(Constraints.MAX_LENGTH_EMAIL);

            builder.HasIndex(x => x.UserName).IsUnique().UseCollation("en_us_ci");
            builder.HasIndex(x => x.Email).IsUnique().UseCollation("en_us_ci");

            builder.HasOne(x => x.Role).WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.HasData(new User { Id = Guid.NewGuid(), PasswordHash = "", PasswordSalt = "",
            CreatedAt = DateTimeOffset.UtcNow, Email = "admin@formscreator.com", IsEmailConfirmed = true,
            IsBlocked = false, UserName = "Default_admin", RoleId = Guid.Parse("7428d1fb-0408-4795-b229-67852851cb0b") });
        }
    }
}
