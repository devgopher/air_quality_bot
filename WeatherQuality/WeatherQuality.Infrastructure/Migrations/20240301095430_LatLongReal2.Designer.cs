﻿// <auto-generated />
using System;
using System.Collections.Generic;
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
    [Migration("20240301095430_LatLongReal2")]
    partial class LatLongReal2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WeatherQuality.Infrastructure.Models.GeoCacheModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("BinaryData")
                        .HasColumnType("bytea");

                    b.Property<string>("ElementName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("SerializedValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("GeoCacheModels");
                });

            modelBuilder.Entity("WeatherQuality.Infrastructure.Models.RequestModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<List<string>>("Current")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<List<string>>("Hourly")
                        .IsRequired()
                        .HasColumnType("text[]");

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

            modelBuilder.Entity("WeatherQuality.Infrastructure.Models.UserLocationModel", b =>
                {
                    b.Property<string>("ChatId")
                        .HasColumnType("text");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.HasKey("ChatId");

                    b.ToTable("UserLocationModels");
                });
#pragma warning restore 612, 618
        }
    }
}
