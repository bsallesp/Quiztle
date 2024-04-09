﻿// <auto-generated />
using System;
using BrunoTheBot.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    [DbContext(typeof(PostgreBrunoTheBotContext))]
    [Migration("20240408174925_AddQuestionInSections")]
    partial class AddQuestionInSections
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Contents");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.School", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ContentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("TopicClassId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ContentId");

                    b.HasIndex("TopicClassId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SchoolId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.ToTable("TopicClasses");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Options");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnswerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Hint")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("SectionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Log.AILog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("JSON")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AILogs");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.Content", "Content")
                        .WithMany()
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", null)
                        .WithMany("Sections")
                        .HasForeignKey("TopicClassId");

                    b.Navigation("Content");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.School", null)
                        .WithMany("Topics")
                        .HasForeignKey("SchoolId");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Option", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", null)
                        .WithMany("Options")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Quiz.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.Section", null)
                        .WithMany("Questions")
                        .HasForeignKey("SectionId");

                    b.Navigation("Answer");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.School", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
