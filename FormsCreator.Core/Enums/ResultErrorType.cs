namespace FormsCreator.Core.Enums
{
    /// <summary>
    /// Enumerator that defines the different types of errors.
    /// </summary>
    public enum ResultErrorType : short
    {
        /// <summary>
        /// Indicates that the error is an error that implicates using null in contexts that aren't allowed.
        /// </summary>
        NullError = 0x854,

        /// <summary>
        /// Error indicating that the operation failed but there is no specific knowledge of what caused the failure.
        /// </summary>
        UnknownError,

        /// <summary>
        /// Error related to database issues, such as the inability to perform a query or a write operation.
        /// </summary>
        DatabaseError,

        /// <summary>
        /// Error related to authorization problems, such as attempting to access a resource without proper permissions.
        /// </summary>
        AuthorizationError,

        /// <summary>
        /// Error related to data validation, for example, when trying to save a form with incorrect data.
        /// </summary>
        ValidationError,

        /// <summary>
        /// Error indicating that a resource was not found, such as when searching for a record that does not exist in the database.
        /// </summary>
        NotFoundError,

        /// <summary>
        /// Error indicating that a resource conflicts with another, for example, when a file with the same name and extension already exists.
        /// </summary>
        ConflictError,

        /// <summary>
        /// Error indicating that the client's request was valid, but the server could not process the request because it cannot meet the conditions of the request.
        /// </summary>
        UnprocessableEntityError,

        /// <summary>
        /// Indicates that there are no errors.
        /// </summary>
        None
    }
}
