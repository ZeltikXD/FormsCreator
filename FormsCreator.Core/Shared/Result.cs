using FormsCreator.Core.Shared.Internal;

namespace FormsCreator.Core.Shared
{
    /// <summary>
    /// Class that stores all possible result types to return after an operation.
    /// </summary>
    public static class Result
    {
        public static IResult Success() => ClassicResult._success;

        public static IResult<TResult> Success<TResult>(TResult value) => new ResultWithValue<TResult>(value, true, ErrorResult.None);

        public static IResult Failure(ErrorResult error) => new ClassicResult(false, error);

        public static IResult<TResult> Failure<TResult>(ErrorResult error) => new ResultWithValue<TResult>(default, false, error);

        public static IResult<T> FailureTo<T>(this IResult result) => Failure<T>(result.Error);
    }
}
