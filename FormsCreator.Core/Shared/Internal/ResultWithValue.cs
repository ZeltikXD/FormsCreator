using System;

namespace FormsCreator.Core.Shared.Internal
{
    internal sealed class ResultWithValue<TValue> : ClassicResult, IResult<TValue>
    {
        private readonly TValue? _value;

        internal ResultWithValue(TValue? value, bool isSuccess, ErrorResult error)
            : base(isSuccess, error) => _value = value;

        public TValue Result => IsSuccess ? _value! :
            throw new InvalidOperationException("The value of a failure result cannot be accessed.");
    }
}
