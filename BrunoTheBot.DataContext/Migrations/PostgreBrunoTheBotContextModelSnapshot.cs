﻿// <auto-generated />
using System;
using BrunoTheBot.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BrunoTheBot.DataContext.Migrations
{
    [DbContext(typeof(PostgreBrunoTheBotContext))]
    partial class PostgreBrunoTheBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Chapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<Guid?>("BookId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Chapters");

                    b.HasAnnotation("Relational:JsonPropertyName", "Chapters");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Content", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Text");

                    b.HasKey("Id");

                    b.ToTable("Contents");

                    b.HasAnnotation("Relational:JsonPropertyName", "Content");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<Guid?>("ChapterId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.HasIndex("ContentId");

                    b.ToTable("Sections");

                    b.HasAnnotation("Relational:JsonPropertyName", "Sections");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.PDFData.PDFData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Description");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "FileName");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.HasKey("Id");

                    b.ToTable("PDFData");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.PDFData.PDFDataPages", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Content");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<Guid?>("PDFDataId")
                        .HasColumnType("uuid");

                    b.Property<int>("Page")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Page");

                    b.HasKey("Id");

                    b.HasIndex("PDFDataId");

                    b.ToTable("PDFDataPages");

                    b.HasAnnotation("Relational:JsonPropertyName", "Pages");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Option", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Options");

                    b.HasAnnotation("Relational:JsonPropertyName", "Options");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Answer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("Hint")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Hint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<string>("Resolution")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Resolution");

                    b.Property<Guid?>("SectionId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TestId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.HasIndex("TestId");

                    b.ToTable("Questions");

                    b.HasAnnotation("Relational:JsonPropertyName", "Questions");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("PDFDataId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PDFDataId");

                    b.ToTable("Tests");

                    b.HasAnnotation("Relational:JsonPropertyName", "Tests");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Tasks.BookTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BookTasks");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Log.AILog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("JSON")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "JSON");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.HasKey("Id");

                    b.ToTable("AILogs");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PhoneNumber")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Chapter", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.Book", null)
                        .WithMany("Chapters")
                        .HasForeignKey("BookId");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.Chapter", null)
                        .WithMany("Sections")
                        .HasForeignKey("ChapterId");

                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.Content", "Content")
                        .WithMany()
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Content");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.PDFData.PDFDataPages", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.PDFData.PDFData", null)
                        .WithMany("Pages")
                        .HasForeignKey("PDFDataId");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Option", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", null)
                        .WithMany("Options")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.Section", null)
                        .WithMany("Questions")
                        .HasForeignKey("SectionId");

                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Quiz.Test", null)
                        .WithMany("Questions")
                        .HasForeignKey("TestId");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Test", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.PDFData.PDFData", null)
                        .WithMany("Tests")
                        .HasForeignKey("PDFDataId");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Tasks.BookTask", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Book", b =>
                {
                    b.Navigation("Chapters");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Chapter", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.PDFData.PDFData", b =>
                {
                    b.Navigation("Pages");

                    b.Navigation("Tests");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Navigation("Options");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Test", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
