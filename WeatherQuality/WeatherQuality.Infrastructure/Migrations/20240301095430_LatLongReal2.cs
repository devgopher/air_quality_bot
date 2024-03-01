using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class LatLongReal2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Longitude",
                table: "GeoCacheModels",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "Latitude",
                table: "GeoCacheModels",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "GeoCacheModels",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "GeoCacheModels",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
