using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class tz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TimeZone",
                table: "UserLocationModels",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ScheduleModels",
                columns: table => new
                {
                    ChatId = table.Column<string>(type: "text", nullable: false),
                    Cron = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleModels", x => new { x.ChatId, x.Cron });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleModels");

            migrationBuilder.AlterColumn<string>(
                name: "TimeZone",
                table: "UserLocationModels",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
