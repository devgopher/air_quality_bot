using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class AirDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AirQualityCacheModels",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AirQualityCacheModels");
        }
    }
}
