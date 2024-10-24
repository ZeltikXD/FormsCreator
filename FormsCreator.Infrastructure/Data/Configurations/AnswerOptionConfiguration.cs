using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal class AnswerOptionConfiguration : EntityBaseConfiguration<AnswerOption>
    {
        public override void Configure(EntityTypeBuilder<AnswerOption> builder)
        {
            builder.ToTable("Answers_Options");

            builder.HasIndex(x => x.AnswerId);
            builder.HasIndex(x => x.QuestionOptionId);

            builder.HasIndex(x => new { x.Id, x.AnswerId });

            builder.Property(x => x.Row)
                .HasMaxLength(Constraints.MAX_LENGTH_ROW_COLUMN);

            builder.Property(x => x.Column)
                .HasMaxLength(Constraints.MAX_LENGTH_ROW_COLUMN);

            builder.Property(x => x.Value)
                .HasMaxLength(Constraints.MAX_LENGTH_VALUE);

            builder.HasOne(x => x.Answer).WithMany(x => x.Options)
                .HasForeignKey(x => x.AnswerId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.QuestionOption).WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionOptionId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
