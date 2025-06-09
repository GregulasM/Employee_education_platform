using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eep_backend.Migrations
{
    /// <inheritdoc />
    public partial class hiddenDelMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_articles_achievements_hidden_achievement_id",
                table: "articles");

            migrationBuilder.DropForeignKey(
                name: "fk_modules_achievements_hidden_achievement_id",
                table: "modules");

            migrationBuilder.DropForeignKey(
                name: "fk_tests_courses_course_id",
                table: "tests");

            migrationBuilder.DropIndex(
                name: "ix_modules_hidden_achievement_id",
                table: "modules");

            migrationBuilder.DropIndex(
                name: "ix_articles_hidden_achievement_id",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "hidden_achievement_id",
                table: "modules");

            migrationBuilder.DropColumn(
                name: "hidden_achievement_id",
                table: "articles");

            migrationBuilder.AlterColumn<int>(
                name: "course_id",
                table: "tests",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_tests_courses_course_id",
                table: "tests",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tests_courses_course_id",
                table: "tests");

            migrationBuilder.AlterColumn<int>(
                name: "course_id",
                table: "tests",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "hidden_achievement_id",
                table: "modules",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "hidden_achievement_id",
                table: "articles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_modules_hidden_achievement_id",
                table: "modules",
                column: "hidden_achievement_id");

            migrationBuilder.CreateIndex(
                name: "ix_articles_hidden_achievement_id",
                table: "articles",
                column: "hidden_achievement_id");

            migrationBuilder.AddForeignKey(
                name: "fk_articles_achievements_hidden_achievement_id",
                table: "articles",
                column: "hidden_achievement_id",
                principalTable: "achievements",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_modules_achievements_hidden_achievement_id",
                table: "modules",
                column: "hidden_achievement_id",
                principalTable: "achievements",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_tests_courses_course_id",
                table: "tests",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id");
        }
    }
}
