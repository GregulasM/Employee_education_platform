using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eep_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAchievement2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "user_achievements",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "user_achievements");
        }
    }
}
