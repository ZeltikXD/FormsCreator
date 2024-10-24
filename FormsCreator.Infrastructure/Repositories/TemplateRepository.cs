using FormsCreator.Application.Resources;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data;
using FormsCreator.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class TemplateRepository(FormsDbContext context, ILogger<TemplateRepository> logger)
        : RepositoryBase<Template>(context, logger), ITemplateRepository
    {
        public Task<IResult<long>> CountAllAsync(CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Templates.LongCountAsync(token));

        public Task<IResult<long>> CountByCreatorAsync(Guid userId, CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Templates.LongCountAsync(x => x.CreatorId == userId, token));

        public Task<IResult<long>> CountByTopicsAsync(string[] topics, CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Templates.LongCountAsync(ContainsTopics(topics), token));

        public Task<IResult<long>> CountByTagsAsync(string[] tags, CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Templates.LongCountAsync(ContainsTags(tags), token));

        public Task<IResult<long>> CountByTextAsync(string text, SupportedLang lang, CancellationToken token = default)
            => ExecuteCountAsync(() =>
            {
                const string query = @"SELECT COUNT(DISTINCT t.""Id"") AS ""Value""
                FROM ""Templates"" t
                LEFT JOIN ""Comments"" c ON t.""Id"" = c.""TemplateId""
                LEFT JOIN ""Questions"" q ON t.""Id"" = q.""TemplateId""
                WHERE
                    to_tsvector(@lang::regconfig, t.""Title"" || ' ' || t.""Description"") @@ plainto_tsquery(@lang::regconfig, @text) OR
                    to_tsvector(@lang::regconfig, c.""Content"") @@ plainto_tsquery(@lang::regconfig, @text) OR
                    to_tsvector(@lang::regconfig, q.""Text"") @@ plainto_tsquery(@lang::regconfig, @text) OR
                    to_tsvector(@lang::regconfig, q.""Description"") @@ plainto_tsquery(@lang::regconfig, @text)";

                return _context.Database
                .SqlQueryRaw<long>(query, new NpgsqlParameter("@lang", GetLang(lang)), new NpgsqlParameter("@text", text)).SingleAsync();
            });

        public Task<IResult<Guid>> CreateAsync(Template entity)
            => ExecuteAddAsync(async () =>
            {
                _context.Templates.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        public Task<IResult> DeleteAsync(Guid id)
            => ExecuteDeleteAsync(async () =>
            {
                var current = await FindAsync(id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, InternalMessages.TemplateNotFoundMessage));
                _context.Templates.Remove(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, InternalMessages.TemplateNotDeleted));
            });

        Task<IResult<Template>> ITemplateRepository.FindAsync(Guid id, CancellationToken token)
            => ExecuteFindAsync(async () =>
            {
                var template = await FindAsync(id, token);
                if (template is null) return Result.Failure<Template>(new(ResultErrorType.NotFoundError, InternalMessages.TemplateNotFoundMessage));
                return Result.Success(template);
            });

        public Task<IResult<IEnumerable<Template>>> GetAllAsync(int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery().OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<Template>>> GetByCreatorAsync(Guid userId, int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery()
                    .Where(x => x.CreatorId == userId).OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<Template>>> GetByTextAsync(int page, int size, string text, SupportedLang lang, CancellationToken token = default)
            => ExecuteGetAsync(async () =>
            {
                const string query = @"SELECT DISTINCT t.*
                FROM ""Templates"" t
                LEFT JOIN ""Comments"" c ON t.""Id"" = c.""TemplateId""
                LEFT JOIN ""Questions"" q ON t.""Id"" = q.""TemplateId""
                WHERE
                    to_tsvector(@lang::regconfig, t.""Title"" || ' ' || t.""Description"") @@ plainto_tsquery(@lang::regconfig, @text) OR
                    to_tsvector(@lang::regconfig, c.""Content"") @@ plainto_tsquery(@lang::regconfig, @text) OR
                    to_tsvector(@lang::regconfig, q.""Text"") @@ plainto_tsquery(@lang::regconfig, @text) OR
                    to_tsvector(@lang::regconfig, q.""Description"") @@ plainto_tsquery(@lang::regconfig, @text)";

                return await _context.Templates
                    .FromSqlRaw(query, new NpgsqlParameter("@lang", GetLang(lang)), new NpgsqlParameter("@text", text))
                    .Include(x => x.Topic).Include(x => x.Creator).Include(x => x.Tags).OrderBy(x => x.CreatedAt)
                    .Skip((page - 1) * size).Take(size).ToListAsync(token);
            });

        public Task<IResult<IEnumerable<Template>>> GetByTopicsAsync(int page, int size, string[] topics, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery().Where(ContainsTopics(topics))
                .OrderByDescending(x => x.CreatedAt).Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<Template>>> GetByTagsAsync(int page, int size, string[] tags, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery().Where(ContainsTags(tags))
                .OrderByDescending(x => x.CreatedAt).Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<Template>>> GetMostPopularAsync(int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery().OrderByDescending(x => x.Forms.LongCount())
                    .Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult> UpdateAsync(Template template)
            => ExecuteUpdateAsync(async () =>
            {
                var current = await FindAsync(template.Id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, InternalMessages.TemplateNotFoundMessage));
                current.Title = template.Title;
                current.Description = template.Description;
                current.Image_Url = template.Image_Url ?? current.Image_Url;
                current.IsPublic = template.IsPublic;
                current.TopicId = template.TopicId;
                current.Tags = template.Tags;
                _context.Templates.Update(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, InternalMessages.TemplateNotUpdated));
            });

        private static string GetLang(SupportedLang lang)
            => lang switch
            {
                SupportedLang.en_US => "english",
                SupportedLang.es_MX => "spanish",
                _ => throw new NotImplementedException()
            };

        private static Expression<Func<Template, bool>> ContainsTopics(string[] topics)
            => template => topics.Any(topic => template.Topic.Name.ToLower() == topic.ToLower());

        private static Expression<Func<Template, bool>> ContainsTags(string[] tags)
            => template => tags.Any(tag => template.Tags.Any(x => x.Name.ToLower() == tag.ToLower()));

        private Task<Template?> FindAsync(Guid id, CancellationToken token = default)
            => BaseQuery().FirstOrDefaultAsync(x => x.Id == id, token);

        public async Task<IResult> HasFormsFilledOutAsync(Guid id, CancellationToken token = default)
            => await ExecuteFindAsync(async () =>
            {
                var res = await _context.Templates.Where(x => x.Id == id).AnyAsync(x => x.Forms.LongCount() > 0);
                return !res ? Result.Success<Template>(null!)
                    : Result.Failure<Template>(new(ResultErrorType.ValidationError, string.Empty));
            });

        IQueryable<Template> BaseQuery()
            => _context.Templates.Include(x => x.Topic).Include(x => x.Creator).Include(x => x.Tags);
    }
}
