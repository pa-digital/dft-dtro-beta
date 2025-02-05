﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DfT.DTRO.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace DfT.DTRO.Migrations
{
    [DbContext(typeof(DtroContext))]
    [Migration("20250205155046_NewDatabaseStructure")]
    partial class NewDatabaseStructure
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.Application", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationPurposeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationTypeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DigitalServiceProviderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Nickname")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<Guid>("TrafficRegulationAuthorityId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.ApplicationPurpose", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ApplicationPurposes");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.ApplicationType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ApplicationTypes");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.DigitalServiceProvider", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DigitalServiceProviders");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.DTRO", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ApplicationId")
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

                    b.Property<List<string>>("RegulatedPlaceTypes")
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

                    b.HasIndex("ApplicationId");

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

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.DtroUser", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("text");

                    b.Property<string>("Prefix")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int?>("TraId")
                        .HasColumnType("integer");

                    b.Property<int>("UserGroup")
                        .HasColumnType("integer");

                    b.Property<Guid>("xAppId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("DtroUsers");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.Metric", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<int>("DeletionCount")
                        .HasColumnType("integer");

                    b.Property<Guid>("DtroUserId")
                        .HasColumnType("uuid");

                    b.Property<int>("EventCount")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("ForDate")
                        .HasColumnType("date");

                    b.Property<string>("LastUpdatedCorrelationId")
                        .HasColumnType("text");

                    b.Property<int>("SearchCount")
                        .HasColumnType("integer");

                    b.Property<int>("SubmissionCount")
                        .HasColumnType("integer");

                    b.Property<int>("SubmissionFailureCount")
                        .HasColumnType("integer");

                    b.Property<int>("SystemFailureCount")
                        .HasColumnType("integer");

                    b.Property<int>("UserGroup")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.RuleTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ApplicationId")
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

                    b.HasIndex("ApplicationId");

                    b.ToTable("RuleTemplate");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.SchemaTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ApplicationId")
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

                    b.HasIndex("ApplicationId");

                    b.ToTable("SchemaTemplate");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.SystemConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsTest")
                        .HasColumnType("boolean");

                    b.Property<string>("SystemName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("SystemConfig");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.TrafficRegulationAuthority", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TrafficRegulationAuthorities");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.TrafficRegulationAuthorityDigitalServiceProvider", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DigitalServiceProviderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TrafficRegulationAuthorityDigitalServiceProviderStatusId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TrafficRegulationAuthorityId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("TrafficRegulationAuthorityDigitalServiceProviders");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.TrafficRegulationAuthorityDigitalServiceProviderStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TrafficRegulationAuthorityDigitalServiceProviderStatuses");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Forename")
                        .HasColumnType("text");

                    b.Property<bool>("IsCentralServiceOperator")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<Guid>("UserStatusId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.UserStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserStatuses");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.DigitalServiceProvider", b =>
                {
                    b.HasOne("DfT.DTRO.Models.DataBase.User", null)
                        .WithMany("DigitalServiceProviders")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.DTRO", b =>
                {
                    b.HasOne("DfT.DTRO.Models.DataBase.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.RuleTemplate", b =>
                {
                    b.HasOne("DfT.DTRO.Models.DataBase.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.SchemaTemplate", b =>
                {
                    b.HasOne("DfT.DTRO.Models.DataBase.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId");

                    b.Navigation("Application");
                });

            modelBuilder.Entity("DfT.DTRO.Models.DataBase.User", b =>
                {
                    b.Navigation("DigitalServiceProviders");
                });
#pragma warning restore 612, 618
        }
    }
}
