using FormsCreator.Core.Models;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class FormConfiguration : EntityUpdateConfiguration<Form>
    {
        public override void Configure(EntityTypeBuilder<Form> builder)
        {
            base.Configure(builder);

            builder.ToTable("Forms");
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.TemplateId);

            builder.HasOne(x => x.User).WithMany(x => x.Forms)
                .HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Template).WithMany(x => x.Forms)
                .HasForeignKey(x => x.TemplateId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
