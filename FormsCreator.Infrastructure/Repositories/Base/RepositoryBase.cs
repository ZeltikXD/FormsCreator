using EntityFramework.Exceptions.Common;
using FormsCreator.Application.Resources;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace FormsCreator.Infrastructure.Repositories.Base
{
    internal abstract class RepositoryBase<TEntity> where TEntity : class
    {
        protected readonly FormsDbContext _context;
        protected readonly ILogger _logger;
        private readonly string _entityName;

        protected internal RepositoryBase(FormsDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            _entityName = typeof(TEntity).Name.ToLower();
        }

        protected void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, "{message}.\nDate: {date}", message, DateTimeOffset.UtcNow.ToString("G"));
        }

        protected void LogWarning(string message, Exception? ex = null)
        {
            _logger.LogWarning(ex, "{message}.\nDate: {date}", message, DateTimeOffset.UtcNow.ToString("G"));
        }

        protected static void CheckIfIsTaskCanceled(Exception ex)
        {
            if (ex.GetType() == typeof(TaskCanceledException) || ex.GetType() == typeof(OperationCanceledException))
                throw ex;
        }

        /// <summary>
        /// This method envelopes a giant try-catch with the possible errors that have origin from the database.
        /// This method is for the <see cref="IRepositoryBase{T}.CreateAsync(T)"/> implementations.
        /// </summary>
        /// <param name="exec">The function to execute.</param>
        /// <param name="_entityName">The name of the entity that will appear in the log messages.</param>
        /// <returns>An operation result.</returns>
        protected async Task<IResult<Guid>> ExecuteAddAsync(Func<Task<IResult<Guid>>> exec)
        {
            try
            {
                return await exec();
            }
            catch (CannotInsertNullException ex)
            {
                LogWarning($"A user attemtepd to create a(n) {_entityName} without data", ex);
                return Result.Failure<Guid>(new(ResultErrorType.UnprocessableEntityError, string.Format(InternalMessages.AddCannotInsertNullEx, _entityName)));
            }
            catch (MaxLengthExceededException ex)
            {
                LogWarning("A user exceeded the maximun of characters in some field", ex);
                return Result.Failure<Guid>(new(ResultErrorType.UnprocessableEntityError, string.Format(InternalMessages.AddMaxLengthEx, _entityName, string.Join(", ", ex.Data.Keys))));
            }
            catch (ReferenceConstraintException ex)
            {
                LogWarning("A property violated the foreign key constraint.", ex);
                return Result.Failure<Guid>(new(ResultErrorType.UnprocessableEntityError, string.Format(InternalMessages.AddRefConstraintEx, _entityName, string.Join(", ", ex.ConstraintProperties))));
            }
            catch (Exception ex)
            {
                LogError($"An error occurred while trying to add a new {_entityName}", ex);
                return Result.Failure<Guid>(new(ResultErrorType.DatabaseError, string.Format(InternalMessages.AddInternalErrorEx, _entityName)));
            }
        }

        /// <summary>
        /// This method envelopes a giant try-catch with the possible errors that have origin from the database.
        /// This method is for the entities that implements any complete update method.
        /// </summary>
        /// <param name="exec">The function to execute.</param>
        /// <returns>An operation result.</returns>
        protected async Task<IResult> ExecuteUpdateAsync(Func<Task<IResult>> exec)
        {
            try
            {
                return await exec();
            }
            catch (CannotInsertNullException ex)
            {
                LogWarning($"A user attempted to update a(n) {_entityName} without data", ex);
                return Result.Failure(new(ResultErrorType.UnprocessableEntityError, string.Format(InternalMessages.UpdCannotInsertNullEx, _entityName)));
            }
            catch (MaxLengthExceededException ex)
            {
                LogWarning("A user exceeded the maximun of characters in some field", ex);
                return Result.Failure(new(ResultErrorType.UnprocessableEntityError, string.Format(InternalMessages.UpdMaxLengthEx, _entityName, string.Join(", ", ex.Data.Keys))));
            }
            catch (ReferenceConstraintException ex)
            {
                LogWarning("A property violated the foreign key constraint.", ex);
                return Result.Failure(new(ResultErrorType.UnprocessableEntityError, string.Format(InternalMessages.UpdRefConstraintEx, _entityName, string.Join(", ", ex.ConstraintProperties))));
            }
            catch (Exception ex)
            {
                LogError($"An error occurred while trying to update the {_entityName}", ex);
                return Result.Failure(new(ResultErrorType.DatabaseError, string.Format(InternalMessages.UpdInternalErrorEx, _entityName)));
            }
        }

        protected async Task<IResult<IEnumerable<TEntity>>> ExecuteGetAsync(Func<Task<IEnumerable<TEntity>>> exec)
        {
            try
            {
                var result = await exec();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return CommonMessageWhenErrorFetching(ex);
            }
        }

        protected async Task<IResult<TEntity>> ExecuteFindAsync(Func<Task<IResult<TEntity>>> exec)
        {
            try
            {
                return await exec();
            }
            catch (Exception ex)
            {
                LogError($"An error occurred while getting a specific {_entityName}", ex);
                return Result.Failure<TEntity>(new(ResultErrorType.DatabaseError, string.Format(InternalMessages.FetchDataException, _entityName)));
            }
        }

        protected async Task<IResult<long>> ExecuteCountAsync(Func<Task<long>> exec)
        {
            try
            {
                var result = await exec();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return CommonMessageWhenErrorCounting(ex);
            }
        }

        protected async Task<IResult> ExecuteDeleteAsync(Func<Task<IResult>> exec)
        {
            try
            {
                return await exec();
            }
            catch (Exception ex)
            {
                LogError($"An error occurred while trying to delete a(n) {_entityName}", ex);
                return Result.Failure(new(ResultErrorType.DatabaseError, string.Format(InternalMessages.DeleteException, _entityName)));
            }
        }

        protected IResult<long> CommonMessageWhenErrorCounting(Exception ex)
        {
            CheckIfIsTaskCanceled(ex);
            LogError($"An error occurred while counting data of type {_entityName}", ex);
            return Result.Failure<long>(new(ResultErrorType.DatabaseError, string.Format(InternalMessages.CountingError, _entityName)));
        }

        protected IResult<IEnumerable<TEntity>> CommonMessageWhenErrorFetching(Exception ex)
        {
            CheckIfIsTaskCanceled(ex);
            LogError($"An error occurred while trying to get data of type {_entityName}", ex);
            return Result.Failure<IEnumerable<TEntity>>(new(ResultErrorType.DatabaseError, string.Format(InternalMessages.FetchDataException, _entityName)));
        }
    }
}
