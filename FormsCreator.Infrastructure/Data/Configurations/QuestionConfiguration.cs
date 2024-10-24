using FormsCreator.Core.Enums;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class QuestionConfiguration : EntityBaseConfiguration<Question>
    {
        public override void Configure(EntityTypeBuilder<Question> builder)
        {
            base.Configure(builder);

            builder.ToTable("Questions");

            builder.HasIndex(x => x.Text)
                .IsTsVectorExpressionIndex("english")
                .HasMethod("gin")
                .HasDatabaseName("index_question_text_search_english");

            builder.HasIndex(x => x.Description)
                .IsTsVectorExpressionIndex("english")
                .HasMethod("GIN")
                .HasDatabaseName("index_question_desc_search_english");

            //Reminder: Add the index for spanish manually in the migration.
            //builder.HasIndex(x => x.Text)
            //    .IsTsVectorExpressionIndex("spanish")
            //    .HasMethod("gin")
            //    .HasDatabaseName("index_question_text_search_spanish");

            builder.Property(x => x.Description).HasMaxLength(Constraints.MAX_LENGTH_QUESTION_DESC);
            builder.Property(x => x.Text).HasMaxLength(Constraints.MAX_LENGTH_QUESTION_TEXT);

            builder.Property(x => x.Type)
                .HasConversion(x => x.ToString(), x => Enum.Parse<QuestionType>(x));

            builder.HasOne(x => x.Template).WithMany(x => x.Questions)
                .HasForeignKey(x => x.TemplateId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
