using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FormsCreator.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameTrigramSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_Users_UserName",
            //    table: "Users");
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("153be8a1-91f2-4850-a5d6-89a8bffbadc3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("67347b68-508d-468c-a383-a16478ad794c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("c6187d27-9ba8-4c07-be76-9ba2e9e61f8f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b24bdd4f-c86c-4a8f-903b-92aabc7c77cd"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d0224d8-761f-4e9f-bc37-4b56fe209fcd"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2024, 10, 24, 1, 39, 29, 326, DateTimeKind.Unspecified).AddTicks(5283), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7428d1fb-0408-4795-b229-67852851cb0b"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2024, 10, 24, 1, 39, 29, 326, DateTimeKind.Unspecified).AddTicks(5285), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("6b87ee5d-5fe3-4ab5-bc2e-6f7ebd4fa8b3"), new DateTimeOffset(new DateTime(2024, 10, 24, 1, 39, 29, 329, DateTimeKind.Unspecified).AddTicks(1761), new TimeSpan(0, 0, 0, 0, 0)), "Education" },
                    { new Guid("b6a50719-77c6-4c3e-b204-8be53dbf1f8c"), new DateTimeOffset(new DateTime(2024, 10, 24, 1, 39, 29, 329, DateTimeKind.Unspecified).AddTicks(1762), new TimeSpan(0, 0, 0, 0, 0)), "Quiz" },
                    { new Guid("f86e3abe-b2d2-439c-ba92-aec1e5879d98"), new DateTimeOffset(new DateTime(2024, 10, 24, 1, 39, 29, 329, DateTimeKind.Unspecified).AddTicks(1764), new TimeSpan(0, 0, 0, 0, 0)), "Other" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsBlocked", "IsEmailConfirmed", "PasswordHash", "PasswordSalt", "RoleId", "UserName" },
                values: new object[] { new Guid("c55e34a1-0686-42ff-a353-7c69e91eddcc"), new DateTimeOffset(new DateTime(2024, 10, 24, 1, 39, 29, 329, DateTimeKind.Unspecified).AddTicks(4797), new TimeSpan(0, 0, 0, 0, 0)), "admin@formscreator.com", false, true, "", "", new Guid("7428d1fb-0408-4795-b229-67852851cb0b"), "Default_admin" });

            migrationBuilder.CreateIndex(
                name: "Index_Users_UserName_Trigram",
                table: "Users",
                column: "UserName",
                unique: false)
                .Annotation("Npgsql:CreatedConcurrently", true)
                .Annotation("Npgsql:IndexMethod", "GIN")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" })
                .Annotation("Relational:Collation", new[] { "en_us_ci" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_Users_UserName_Trigram",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("6b87ee5d-5fe3-4ab5-bc2e-6f7ebd4fa8b3"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("b6a50719-77c6-4c3e-b204-8be53dbf1f8c"));

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: new Guid("f86e3abe-b2d2-439c-ba92-aec1e5879d98"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c55e34a1-0686-42ff-a353-7c69e91eddcc"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d0224d8-761f-4e9f-bc37-4b56fe209fcd"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2024, 10, 21, 23, 3, 24, 301, DateTimeKind.Unspecified).AddTicks(1483), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7428d1fb-0408-4795-b229-67852851cb0b"),
                column: "CreatedAt",
                value: new DateTimeOffset(new DateTime(2024, 10, 21, 23, 3, 24, 301, DateTimeKind.Unspecified).AddTicks(1486), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("153be8a1-91f2-4850-a5d6-89a8bffbadc3"), new DateTimeOffset(new DateTime(2024, 10, 21, 23, 3, 24, 304, DateTimeKind.Unspecified).AddTicks(2537), new TimeSpan(0, 0, 0, 0, 0)), "Other" },
                    { new Guid("67347b68-508d-468c-a383-a16478ad794c"), new DateTimeOffset(new DateTime(2024, 10, 21, 23, 3, 24, 304, DateTimeKind.Unspecified).AddTicks(2521), new TimeSpan(0, 0, 0, 0, 0)), "Education" },
                    { new Guid("c6187d27-9ba8-4c07-be76-9ba2e9e61f8f"), new DateTimeOffset(new DateTime(2024, 10, 21, 23, 3, 24, 304, DateTimeKind.Unspecified).AddTicks(2522), new TimeSpan(0, 0, 0, 0, 0)), "Quiz" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsBlocked", "IsEmailConfirmed", "PasswordHash", "PasswordSalt", "RoleId", "UserName" },
                values: new object[] { new Guid("b24bdd4f-c86c-4a8f-903b-92aabc7c77cd"), new DateTimeOffset(new DateTime(2024, 10, 21, 23, 3, 24, 304, DateTimeKind.Unspecified).AddTicks(6028), new TimeSpan(0, 0, 0, 0, 0)), "admin@formscreator.com", false, true, "", "", new Guid("7428d1fb-0408-4795-b229-67852851cb0b"), "Default_admin" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_UserName",
            //    table: "Users",
            //    column: "UserName",
            //    unique: true)
            //    .Annotation("Relational:Collation", new[] { "en_us_ci" });
        }
    }
}
