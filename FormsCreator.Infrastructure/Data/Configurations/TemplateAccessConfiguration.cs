using FormsCreator.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class TemplateAccessConfiguration : IEntityTypeConfiguration<TemplateAccess>
    {
        public void Configure(EntityTypeBuilder<TemplateAccess> builder)
        {
            builder.ToTable("Template_Accesses");

            builder.HasKey(x => new { x.TemplateId, x.UserId });
            builder.HasIndex(x => new { x.TemplateId, x.UserId }).IsUnique();

            builder.HasOne(x => x.User).WithMany(x => x.TemplateAccesses)
                .HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Template).WithMany(x => x.UsersAllowed)
                .HasForeignKey(x => x.TemplateId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
