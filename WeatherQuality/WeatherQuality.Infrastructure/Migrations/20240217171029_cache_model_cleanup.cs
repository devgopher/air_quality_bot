using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class cache_model_cleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirQualityCacheModels_RequestModels_RequestModelId",
                table: "AirQualityCacheModels");

            migrationBuilder.DropIndex(
                name: "IX_AirQualityCacheModels_RequestModelId",
                table: "AirQualityCacheModels");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AirQualityCacheModels");

            migrationBuilder.DropColumn(
                name: "RequestModelId",
                table: "AirQualityCacheModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "AirQualityCacheModels",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<Guid>(
                name: "RequestModelId",
                table: "AirQualityCacheModels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AirQualityCacheModels_RequestModelId",
                table: "AirQualityCacheModels",
                column: "RequestModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AirQualityCacheModels_RequestModels_RequestModelId",
                table: "AirQualityCacheModels",
                column: "RequestModelId",
                principalTable: "RequestModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
