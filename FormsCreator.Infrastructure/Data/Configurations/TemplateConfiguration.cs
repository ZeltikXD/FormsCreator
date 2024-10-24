using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using FormsCreator.Infrastructure.Data.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class TemplateConfiguration : EntityUpdateConfiguration<Template>
    {
        public override void Configure(EntityTypeBuilder<Template> builder)
        {
            base.Configure(builder);

            builder.ToTable("Templates");

            builder.HasIndex(x => new { x.Title, x.Description })
                .IsTsVectorExpressionIndex("english")
                .HasMethod("gin")
                .HasDatabaseName("index_template_text_search_english");

            //Reminder: Add the index for spanish manually in the migration.
            //builder.HasIndex(x => new { x.Title, x.Description })
            //    .IsTsVectorExpressionIndex("spanish")
            //    .HasMethod("gin")
            //    .HasDatabaseName("index_template_text_search_spanish");

            builder.HasIndex(x => x.CreatorId);

            builder.Property(x => x.Title)
                .HasMaxLength(Constraints.MAX_LENGTH_TEMPL_TITLE);

            builder.Property(x => x.Description)
                .HasMaxLength(Constraints.MAX_LENGTH_TEMPL_DESC);

            builder.Property(x => x.Image_Url)
                .HasMaxLength(Constraints.MAX_LENGTH_IMAGE_URL);

            builder.HasOne(x => x.Creator).WithMany(x => x.Templates)
                .HasForeignKey(x => x.CreatorId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Topic).WithMany(x => x.Templates)
                .HasForeignKey(x => x.TopicId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Tags).WithMany(x => x.Templates)
                .UsingEntity<TemplateTag>(
                r => r.HasOne<Tag>().WithMany().HasForeignKey(x => x.TagId).IsRequired().OnDelete(DeleteBehavior.Cascade),
                l => l.HasOne<Template>().WithMany().HasForeignKey(x => x.TemplateId).IsRequired().OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.ToTable("Templates_Tags");
                    j.HasKey(x => new { x.TagId, x.TemplateId });
                    j.HasIndex(x => new { x.TagId, x.TemplateId }).IsUnique();
                });
        }
    }
}
