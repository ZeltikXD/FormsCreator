using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FormsCreator.Infrastructure.Data.ValueGenerators
{
    internal sealed class UtcDateTimeValueGenerator : ValueGenerator<DateTimeOffset>
    {
        public UtcDateTimeValueGenerator() { }

        public override DateTimeOffset Next(EntityEntry entry) => DateTimeOffset.UtcNow;

        public override ValueTask<DateTimeOffset> NextAsync(EntityEntry entry,
            CancellationToken cancellationToken = default) =>
            ValueTask.FromResult(DateTimeOffset.UtcNow);

        public override bool GeneratesTemporaryValues => false;
    }
}
