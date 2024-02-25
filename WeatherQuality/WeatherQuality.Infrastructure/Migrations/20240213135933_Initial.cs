using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Current = table.Column<string>(type: "text", nullable: false),
                    Hourly = table.Column<string>(type: "text", nullable: false),
                    Timezone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLocationModels",
                columns: table => new
                {
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocationModels", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "AirQualityCacheModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestModelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Radius = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQualityCacheModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirQualityCacheModels_RequestModels_RequestModelId",
                        column: x => x.RequestModelId,
                        principalTable: "RequestModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirQualityCacheModels_RequestModelId",
                table: "AirQualityCacheModels",
                column: "RequestModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirQualityCacheModels");

            migrationBuilder.DropTable(
                name: "UserLocationModels");

            migrationBuilder.DropTable(
                name: "RequestModels");
        }
    }
}
