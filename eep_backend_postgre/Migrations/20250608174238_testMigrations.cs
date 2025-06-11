using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eep_backend.Migrations
{
    /// <inheritdoc />
    public partial class testMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attempts",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "available_from",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "available_to",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "max_score",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "passing_score",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "randomize",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "tags",
                table: "tests");

            migrationBuilder.DropColumn(
                name: "time_limit",
                table: "tests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "attempts",
                table: "tests",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "available_from",
                table: "tests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "available_to",
                table: "tests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "max_score",
                table: "tests",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "passing_score",
                table: "tests",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "randomize",
                table: "tests",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tags",
                table: "tests",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "time_limit",
                table: "tests",
                type: "double precision",
                nullable: true);
        }
    }
}
