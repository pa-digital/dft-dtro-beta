﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DfT.DTRO.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace DfT.DTRO.Migrations
{
    [DbContext(typeof(DtroContext))]
    partial class DtroContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.DTRO", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<NpgsqlBox>("Location")
                        .HasColumnType("box");

                    b.Property<List<string>>("OrderReportingPoints")
                        .HasColumnType("text[]");

                    b.Property<DateTime?>("RegulationEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RegulationStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<List<string>>("RegulationTypes")
                        .HasColumnType("text[]");

                    b.Property<string>("SchemaVersion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TrafficAuthorityCreatorId")
                        .HasColumnType("integer")
                        .HasColumnName("TraCreator");

                    b.Property<int>("TrafficAuthorityOwnerId")
                        .HasColumnType("integer")
                        .HasColumnName("CurrentTraOwner");

                    b.Property<string>("TroName")
                        .HasColumnType("text");

                    b.Property<List<string>>("VehicleTypes")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Dtros");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.DTROHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DtroId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SchemaVersion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TrafficAuthorityCreatorId")
                        .HasColumnType("integer")
                        .HasColumnName("TraCreator");

                    b.Property<int>("TrafficAuthorityOwnerId")
                        .HasColumnType("integer")
                        .HasColumnName("CurrentTraOwner");

                    b.HasKey("Id");

                    b.ToTable("DtroHistories");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.RuleTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<string>("SchemaVersion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RuleTemplate");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.SchemaTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<string>("SchemaVersion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.ToTable("SchemaTemplate");
                });
#pragma warning restore 612, 618
        }
    }
}
