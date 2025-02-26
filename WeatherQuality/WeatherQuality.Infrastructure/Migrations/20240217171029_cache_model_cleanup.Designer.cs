﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeatherQuality.Infrastructure;

#nullable disable

namespace WeatherQuality.Telegram.Migrations
{
    [DbContext(typeof(WeatherQualityContext))]
    [Migration("20240217171029_cache_model_cleanup")]
    partial class cache_model_cleanup
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WeatherQuality.Telegram.Database.Models.AirQualityCacheModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Radius")
                        .HasColumnType("double precision");

                    b.Property<string>("SerializedResponse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("AirQualityCacheModels");
                });

            modelBuilder.Entity("WeatherQuality.Telegram.Database.Models.RequestModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Current")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Hourly")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("numeric");

                    b.Property<string>("Timezone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RequestModels");
                });

            modelBuilder.Entity("WeatherQuality.Telegram.Database.Models.UserLocationModel", b =>
                {
                    b.Property<string>("ChatId")
                        .HasColumnType("text");

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("numeric");

                    b.HasKey("ChatId");

                    b.ToTable("UserLocationModels");
                });
#pragma warning restore 612, 618
        }
    }
}
