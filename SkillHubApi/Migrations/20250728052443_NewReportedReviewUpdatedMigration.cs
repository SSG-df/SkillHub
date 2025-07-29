using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillHubApi.Migrations
{
    /// <inheritdoc />
    public partial class NewReportedReviewUpdatedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "ReportedReviews",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratorComment",
                table: "ReportedReviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedAt",
                table: "ReportedReviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportedReviews_ReporterId",
                table: "ReportedReviews",
                column: "ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportedReviews_Users_ReporterId",
                table: "ReportedReviews",
                column: "ReporterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportedReviews_Users_ReporterId",
                table: "ReportedReviews");

            migrationBuilder.DropIndex(
                name: "IX_ReportedReviews_ReporterId",
                table: "ReportedReviews");

            migrationBuilder.DropColumn(
                name: "ModeratorComment",
                table: "ReportedReviews");

            migrationBuilder.DropColumn(
                name: "ProcessedAt",
                table: "ReportedReviews");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "ReportedReviews",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 1000);
        }
    }
}
