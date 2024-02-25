using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    /// <inheritdoc />
    public partial class cache_model_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirQualityCacheModels_RequestModels_RequestModelId",
                table: "AirQualityCacheModels");

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestModelId",
                table: "AirQualityCacheModels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "AirQualityCacheModels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "AirQualityCacheModels",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "SerializedResponse",
                table: "AirQualityCacheModels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AirQualityCacheModels_RequestModels_RequestModelId",
                table: "AirQualityCacheModels",
                column: "RequestModelId",
                principalTable: "RequestModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AirQualityCacheModels_RequestModels_RequestModelId",
                table: "AirQualityCacheModels");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AirQualityCacheModels");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AirQualityCacheModels");

            migrationBuilder.DropColumn(
                name: "SerializedResponse",
                table: "AirQualityCacheModels");

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestModelId",
                table: "AirQualityCacheModels",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_AirQualityCacheModels_RequestModels_RequestModelId",
                table: "AirQualityCacheModels",
                column: "RequestModelId",
                principalTable: "RequestModels",
                principalColumn: "Id");
        }
    }
}
