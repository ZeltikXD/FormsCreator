using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class AnswerConfiguration : EntityBaseConfiguration<Answer>
    {
        public override void Configure(EntityTypeBuilder<Answer> builder)
        {
            base.Configure(builder);
            builder.ToTable("Answers");

            builder.HasIndex(x => x.FormId);

            builder.HasIndex(x => x.QuestionId);

            builder.HasOne(x => x.Form).WithMany(x => x.Answers)
                .HasForeignKey(x => x.FormId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Question).WithMany(x => x.Answers)
                .HasForeignKey(x => x.QuestionId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
