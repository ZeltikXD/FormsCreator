using FormsCreator.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.ToTable("Likes");

            builder.HasKey(x => new { x.UserId, x.TemplateId });
            builder.HasIndex(x => new { x.UserId, x.TemplateId }).IsUnique();

            builder.Property(x => x.IsDeleted).ValueGeneratedOnAdd()
                .HasDefaultValue(false);

            builder.HasOne(x => x.User).WithMany(x => x.Likes)
                .HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Template).WithMany(x => x.Likes)
                .HasForeignKey(x => x.TemplateId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
