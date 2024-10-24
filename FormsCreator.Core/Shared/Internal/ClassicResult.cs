using System;

namespace FormsCreator.Core.Shared.Internal
{
    internal class ClassicResult : IResult
    {
        protected internal ClassicResult(bool isSuccess, ErrorResult error)
        {
            if (isSuccess && error != ErrorResult.None)
            {
                throw new InvalidOperationException("A successful operation can not contain any errors.");
            }

            if (!isSuccess && error == ErrorResult.None)
            {
                throw new InvalidOperationException("A failed operation must contain errors.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public static readonly ClassicResult _success = new(true, ErrorResult.None);

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public ErrorResult Error { get; }
    }
}
