using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YTShorts.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsToShorts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Shorts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Shorts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Shorts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Shorts",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Shorts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Shorts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Shorts");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Shorts");
        }
    }
}
