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

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Chapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BookId")
                        .HasColumnType("integer");

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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Text");

                    b.HasKey("Id");

                    b.ToTable("Contents");

                    b.HasAnnotation("Relational:JsonPropertyName", "Content");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ChapterId")
                        .HasColumnType("integer");

                    b.Property<int>("ContentId")
                        .HasColumnType("integer");

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

                    b.Property<int?>("SectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Questions");

                    b.HasAnnotation("Relational:JsonPropertyName", "Questions");
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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

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

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
