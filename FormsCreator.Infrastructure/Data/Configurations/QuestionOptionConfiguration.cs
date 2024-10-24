using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data.Configurations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormsCreator.Infrastructure.Data.Configurations
{
    internal sealed class QuestionOptionConfiguration : EntityBaseConfiguration<QuestionOption>
    {
        public override void Configure(EntityTypeBuilder<QuestionOption> builder)
        {
            base.Configure(builder);

            builder.ToTable("QuestionOptions");
            builder.Property(x => x.Value).HasMaxLength(Constraints.MAX_LENGTH_VALUE);

            builder.Property(x => x.Row)
                .HasMaxLength(Constraints.MAX_LENGTH_ROW_COLUMN);

            builder.Property(x => x.Column)
                .HasMaxLength(Constraints.MAX_LENGTH_ROW_COLUMN);

            builder.HasOne(x => x.Question).WithMany(x => x.Options)
                .HasForeignKey(x => x.QuestionId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
