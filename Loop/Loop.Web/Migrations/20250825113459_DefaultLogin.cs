using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loop.Web.Migrations
{
    /// <inheritdoc />
    public partial class DefaultLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Email", "Name", "Password", "UpdatedBy", "UpdatedOn" },
                values: new object[] { 2, "Seed", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "sweety@loop.com", "Ashrita", "$2a$11$p2ycaovI53KAt6CRNJmlVuCz8c8iEwhNUcaGrIspZ1gGNhPHBBiR2", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
