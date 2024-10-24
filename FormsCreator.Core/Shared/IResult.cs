namespace FormsCreator.Core.Shared
{
    public interface IResult
    {
        bool IsSuccess { get; }

        bool IsFailure { get; }

        ErrorResult Error { get; }
    }

    public interface IResult<TResult> : IResult
    {
        TResult Result { get; }
    }
}
