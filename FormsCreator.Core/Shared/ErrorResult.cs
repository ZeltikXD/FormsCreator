using FormsCreator.Core.Enums;
using System;

namespace FormsCreator.Core.Shared
{
    public sealed class ErrorResult : IEquatable<ErrorResult>
    {
        public ErrorResult(ResultErrorType code, string message)
        {
            Code = code;
            Message = message;
        }

        private static readonly ErrorResult _none = new(ResultErrorType.None, string.Empty);
        private static readonly ErrorResult _nullValue = new(ResultErrorType.NullError, "The specified result value is null.");

        public static ErrorResult None => _none;
        public static ErrorResult NullError => _nullValue;

        public ResultErrorType Code { get; }

        public string Message { get; }

        public static implicit operator string(ErrorResult value) => value.Message;

        public static bool operator ==(ErrorResult? a, ErrorResult? b)
        {
            if (a is null && b is null) return true;

            if (a is null || b is null) return false;

            return a.Equals(b);
        }

        public static bool operator !=(ErrorResult? a, ErrorResult? b) => !(a == b);

        public bool Equals(ErrorResult? other)
        {
            if (other is null) return false;

            return Code == other.Code && Message == other.Message;
        }

        public override bool Equals(object? obj) => obj is ErrorResult error && Equals(error);

        public override int GetHashCode() => HashCode.Combine(Code, Message);

        public override string ToString() => Message;
    }
}
