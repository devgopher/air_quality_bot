using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class CacheModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirQualityCacheDetailsModels");

            migrationBuilder.DropTable(
                name: "AirQualityCacheModels");
       
            migrationBuilder.CreateTable(
                name: "GeoCacheModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    ElementName = table.Column<string>(type: "text", nullable: false),
                    SerializedValue = table.Column<string>(type: "text", nullable: false),
                    BinaryData = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoCacheModels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeoCacheModels");

            migrationBuilder.AlterColumn<string>(
                name: "Hourly",
                table: "RequestModels",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "Current",
                table: "RequestModels",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.CreateTable(
                name: "AirQualityCacheDetailsModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<string>(type: "text", nullable: false),
                    Radius = table.Column<double>(type: "double precision", nullable: false),
                    SerializedResponse = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQualityCacheDetailsModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirQualityCacheModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<string>(type: "text", nullable: false),
                    Radius = table.Column<double>(type: "double precision", nullable: false),
                    SerializedResponse = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQualityCacheModels", x => x.Id);
                });
        }
    }
}
