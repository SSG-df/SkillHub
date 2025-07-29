using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillHubApi.Migrations
{
    /// <inheritdoc />
    public partial class ReportRequestUpdatedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Format",
                table: "ReportRequests");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ReportRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonId",
                table: "ReportRequests",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ReportRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "ReportRequests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReportRequests");

            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "ReportRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ReportRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
