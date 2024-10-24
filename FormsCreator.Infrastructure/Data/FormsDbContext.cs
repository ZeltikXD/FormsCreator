using FormsCreator.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FormsCreator.Infrastructure.Data
{
    internal class FormsDbContext(DbContextOptions<FormsDbContext> options) : DbContext(options)
    {
        public DbSet<Answer> Answers { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Form> Forms { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<TemplateAccess> TemplateAccesses { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserProvider> Providers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasCollation("en_us_ci", "en-US-u-ks-level2", "icu", false);
            modelBuilder.HasPostgresExtension("pg_trgm");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FormsDbContext).Assembly);
        }
    }
}
