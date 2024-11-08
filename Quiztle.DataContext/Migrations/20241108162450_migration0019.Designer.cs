﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Quiztle.DataContext;

#nullable disable

namespace Quiztle.DataContext.Migrations
{
    [DbContext(typeof(PostgreQuiztleContext))]
    [Migration("20241108162450_migration0019")]
    partial class migration0019
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BookTask", b =>
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

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("BookTasks");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Book", b =>
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

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Chapter", b =>
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

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Content", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "OriginalContent");

                    b.HasKey("Id");

                    b.ToTable("Contents");

                    b.HasAnnotation("Relational:JsonPropertyName", "Content");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Section", b =>
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

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.PDFData.PDFData", b =>
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

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.PDFData.PDFDataPages", b =>
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

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Paid.Paid", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasAnnotation("Relational:JsonPropertyName", "Amount");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Currency");

                    b.Property<string>("CustomerId")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "CustomerId");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Description");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "LastUpdated");

                    b.Property<string>("PaymentIntentId")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "PaymentIntentId");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "PaymentMethod");

                    b.Property<string>("PriceId")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "PriceId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Status");

                    b.Property<string>("TestId")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "TestId");

                    b.Property<string>("UserEmail")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "UserEmail");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "UserId");

                    b.HasKey("Id");

                    b.ToTable("Paids");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Performance.QuestionsPerformance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<string>("CorrectAnswerName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "CorrectAnswerName");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<string>("IncorrectAnswerName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "IncorrectAnswerName");

                    b.Property<string>("QuestionName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "QuestionName");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "TagName");

                    b.Property<Guid?>("TestPerformanceId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TestPerformanceId");

                    b.ToTable("QuestionsPerformance");

                    b.HasAnnotation("Relational:JsonPropertyName", "QuestionsPerformance");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Performance.TestPerformance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<int>("CorrectAnswers")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "CorrectAnswers");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<int>("IncorrectAnswers")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "IncorrectAnswers");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Score");

                    b.Property<string>("Shield")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Shield");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "TestId");

                    b.Property<string>("TestName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "TestName");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "UserId");

                    b.HasKey("Id");

                    b.ToTable("TestsPerformance");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Prompts.Prompt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Prompts");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Prompts.PromptItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DraftId")
                        .HasColumnType("uuid");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<Guid>("PromptId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("SentenceId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DraftId");

                    b.HasIndex("SentenceId");

                    b.HasIndex("PromptId", "Order")
                        .IsUnique();

                    b.ToTable("PromptItems");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Prompts.Sentence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Sentence");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Option", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean")
                        .HasAnnotation("Relational:JsonPropertyName", "IsCorrect");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "QuestionId");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Options");

                    b.HasAnnotation("Relational:JsonPropertyName", "Options");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<int>("ConfidenceLevel")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "ConfidenceLevel");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<Guid?>("DraftId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "DraftId");

                    b.Property<string>("Hint")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Hint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<int>("Rate")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Rate");

                    b.Property<string>("Resolution")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Resolution");

                    b.Property<Guid?>("SectionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Tag")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Tag");

                    b.Property<Guid?>("TestId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "TestId");

                    b.Property<bool>("Verified")
                        .HasColumnType("boolean")
                        .HasAnnotation("Relational:JsonPropertyName", "Verified");

                    b.Property<int>("VerifiedTimes")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "VerifiedTimes");

                    b.HasKey("Id");

                    b.HasIndex("DraftId");

                    b.HasIndex("SectionId");

                    b.HasIndex("TestId");

                    b.ToTable("Questions");

                    b.HasAnnotation("Relational:JsonPropertyName", "Questions");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Response", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<bool>("IsFinalized")
                        .HasColumnType("boolean")
                        .HasAnnotation("Relational:JsonPropertyName", "IsFinalized");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("numeric")
                        .HasAnnotation("Relational:JsonPropertyName", "Percentage");

                    b.Property<int>("Score")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "Score");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "TestId");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("Responses");

                    b.HasAnnotation("Relational:JsonPropertyName", "Responses");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Shot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<Guid>("OptionId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "OptionId");

                    b.Property<Guid>("ResponseId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "ResponseId");

                    b.HasKey("Id");

                    b.HasIndex("ResponseId");

                    b.ToTable("Shots");

                    b.HasAnnotation("Relational:JsonPropertyName", "Shots");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "Id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasAnnotation("Relational:JsonPropertyName", "Created");

                    b.Property<bool>("IsAvaiable")
                        .HasColumnType("boolean")
                        .HasAnnotation("Relational:JsonPropertyName", "IsAvaiable");

                    b.Property<bool>("IsPremium")
                        .HasColumnType("boolean")
                        .HasAnnotation("Relational:JsonPropertyName", "IsPremium");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "Name");

                    b.Property<Guid?>("PDFDataId")
                        .HasColumnType("uuid")
                        .HasAnnotation("Relational:JsonPropertyName", "PDFDataId");

                    b.Property<string>("PriceId")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "PriceId");

                    b.Property<int>("PriceInCents")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "PriceInCents");

                    b.Property<string>("ShieldSVG")
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "ShieldSVG");

                    b.HasKey("Id");

                    b.HasIndex("PDFDataId");

                    b.ToTable("Tests");

                    b.HasAnnotation("Relational:JsonPropertyName", "Test");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.TestQuestion", b =>
                {
                    b.Property<Guid>("TestId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.HasKey("TestId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("TestsQuestions");

                    b.HasAnnotation("Relational:JsonPropertyName", "TestQuestions");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Scratch.Draft", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("MadeByAiContent")
                        .HasColumnType("text");

                    b.Property<string>("OriginalContent")
                        .HasColumnType("text");

                    b.Property<int>("QuestionsAmountTarget")
                        .HasColumnType("integer");

                    b.Property<Guid?>("ScratchId")
                        .HasColumnType("uuid");

                    b.Property<string>("Tag")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScratchId");

                    b.ToTable("Drafts");

                    b.HasAnnotation("Relational:JsonPropertyName", "Draft");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Scratch.Scratch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Scratches");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Log.AILog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GuidLog")
                        .HasColumnType("uuid");

                    b.Property<string>("JSON")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AILogs");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Log.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("GuidLog")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BookTask", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Chapter", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Course.Book", null)
                        .WithMany("Chapters")
                        .HasForeignKey("BookId");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Course.Chapter", null)
                        .WithMany("Sections")
                        .HasForeignKey("ChapterId");

                    b.HasOne("Quiztle.CoreBusiness.Entities.Course.Content", "Content")
                        .WithMany()
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Content");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.PDFData.PDFDataPages", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.PDFData.PDFData", null)
                        .WithMany("Pages")
                        .HasForeignKey("PDFDataId");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Performance.QuestionsPerformance", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Performance.TestPerformance", null)
                        .WithMany("QuestionsPerformance")
                        .HasForeignKey("TestPerformanceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Prompts.PromptItem", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Scratch.Draft", "Draft")
                        .WithMany()
                        .HasForeignKey("DraftId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Quiztle.CoreBusiness.Entities.Prompts.Prompt", "Prompt")
                        .WithMany("Items")
                        .HasForeignKey("PromptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quiztle.CoreBusiness.Entities.Prompts.Sentence", "Sentence")
                        .WithMany()
                        .HasForeignKey("SentenceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Draft");

                    b.Navigation("Prompt");

                    b.Navigation("Sentence");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Option", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Quiz.Question", "Question")
                        .WithMany("Options")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Scratch.Draft", "Draft")
                        .WithMany("Questions")
                        .HasForeignKey("DraftId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Quiztle.CoreBusiness.Entities.Course.Section", null)
                        .WithMany("Questions")
                        .HasForeignKey("SectionId");

                    b.HasOne("Quiztle.CoreBusiness.Entities.Quiz.Test", "Test")
                        .WithMany("Questions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Draft");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Response", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Quiz.Test", null)
                        .WithMany("Responses")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Shot", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Quiz.Response", null)
                        .WithMany("Shots")
                        .HasForeignKey("ResponseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Test", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.PDFData.PDFData", null)
                        .WithMany("Tests")
                        .HasForeignKey("PDFDataId");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.TestQuestion", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Quiz.Question", "Question")
                        .WithMany("TestQuestions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Quiztle.CoreBusiness.Entities.Quiz.Test", "Test")
                        .WithMany("TestQuestions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Scratch.Draft", b =>
                {
                    b.HasOne("Quiztle.CoreBusiness.Entities.Scratch.Scratch", null)
                        .WithMany("Drafts")
                        .HasForeignKey("ScratchId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Book", b =>
                {
                    b.Navigation("Chapters");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Chapter", b =>
                {
                    b.Navigation("Sections");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Course.Section", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.PDFData.PDFData", b =>
                {
                    b.Navigation("Pages");

                    b.Navigation("Tests");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Performance.TestPerformance", b =>
                {
                    b.Navigation("QuestionsPerformance");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Prompts.Prompt", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Question", b =>
                {
                    b.Navigation("Options");

                    b.Navigation("TestQuestions");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Response", b =>
                {
                    b.Navigation("Shots");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Quiz.Test", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("Responses");

                    b.Navigation("TestQuestions");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Scratch.Draft", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("Quiztle.CoreBusiness.Entities.Scratch.Scratch", b =>
                {
                    b.Navigation("Drafts");
                });
#pragma warning restore 612, 618
        }
    }
}
