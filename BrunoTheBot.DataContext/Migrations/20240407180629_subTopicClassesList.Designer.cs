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
    [Migration("20240407180629_subTopicClassesList")]
    partial class subTopicClassesList
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

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("TopicClassId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TopicClassId");

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

                    b.Property<int?>("TopicClassId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SchoolId");

                    b.HasIndex("TopicClassId");

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

                    b.Property<int>("TopicId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("TopicId");

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

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.Content", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", "TopicClass")
                        .WithMany()
                        .HasForeignKey("TopicClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TopicClass");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", b =>
                {
                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.School", null)
                        .WithMany("Topics")
                        .HasForeignKey("SchoolId");

                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", null)
                        .WithMany("SubTopicClasses")
                        .HasForeignKey("TopicClassId");
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

                    b.HasOne("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.School", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Course.TopicClass", b =>
                {
                    b.Navigation("SubTopicClasses");
                });

            modelBuilder.Entity("BrunoTheBot.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
