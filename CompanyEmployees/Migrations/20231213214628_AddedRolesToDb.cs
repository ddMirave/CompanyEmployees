using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60ace7c1-e3ab-4180-8470-9d4ccf5ee777", "9fd6edf2-0428-49f5-846d-8d9773c868df", "Administrator", "ADMINISTRATOR" },
                    { "cf18acb3-724a-4b41-986d-fca3aa2121d8", "53a857de-f20a-4d0b-b9c2-e392ad81b050", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60ace7c1-e3ab-4180-8470-9d4ccf5ee777");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf18acb3-724a-4b41-986d-fca3aa2121d8");
        }
    }
}
