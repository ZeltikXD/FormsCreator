using FormsCreator.Core.Models;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class UserProviderConfiguration : EntityBaseConfiguration<UserProvider>
    {
        public override void Configure(EntityTypeBuilder<UserProvider> builder)
        {
            base.Configure(builder);

            builder.ToTable("UserProviders");

            builder.HasOne(x => x.User).WithMany(x => x.Providers)
                .HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
