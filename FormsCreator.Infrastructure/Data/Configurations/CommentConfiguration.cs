using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class CommentConfiguration : EntityUpdateConfiguration<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            base.Configure(builder);

            builder.ToTable("Comments");
            builder.Property(x => x.Content).HasMaxLength(Constraints.MAX_LENGTH_COMMENT_CONTENT);

            builder.HasIndex(x => x.Content)
                .IsTsVectorExpressionIndex("english")
                .HasMethod("gin")
                .HasDatabaseName("index_comment_text_search_english");

            //Reminder: Add the index for spanish manually in the migration.
            //builder.HasIndex(x => x.Content)
            //    .IsTsVectorExpressionIndex("spanish")
            //    .HasMethod("gin")
            //    .HasDatabaseName("index_comment_text_search_spanish");

            builder.HasOne(x => x.User).WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Template).WithMany(x => x.Comments)
                .HasForeignKey(x => x.TemplateId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
