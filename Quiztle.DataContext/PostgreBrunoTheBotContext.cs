﻿using Quiztle.CoreBusiness.Entities.Course;
using Quiztle.CoreBusiness.Entities.Quiz;
using Microsoft.EntityFrameworkCore;
using Quiztle.CoreBusiness.Log;
using Quiztle.CoreBusiness.Entities.PDFData;
using Quiztle.CoreBusiness.Entities.Prompts;
using Quiztle.CoreBusiness.Entities.Scratch;
using Quiztle.CoreBusiness;
using Quiztle.CoreBusiness.Entities.Performance;
using Quiztle.CoreBusiness.Entities.Paid;

namespace Quiztle.DataContext
{
    public class PostgreQuiztleContext : DbContext
    {
        public PostgreQuiztleContext(DbContextOptions<PostgreQuiztleContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AILog>();

            modelBuilder.Entity<Book>();

            modelBuilder.Entity<BookTask>()
                .HasOne(bt => bt.User)
                .WithMany()
                .HasForeignKey(bt => bt.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Chapter>();

            modelBuilder.Entity<Content>();

            modelBuilder.Entity<Draft>()
                .HasMany(d => d.Questions)
                .WithOne(q => q.Draft)
                .HasForeignKey(q => q.DraftId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Option>();

            modelBuilder.Entity<PDFData>();

            modelBuilder.Entity<PDFDataPages>();

            modelBuilder.Entity<Prompt>();

            modelBuilder.Entity<PromptItem>()
                .HasOne(pi => pi.Sentence)
                .WithMany()
                .HasForeignKey(pi => pi.SentenceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PromptItem>()
                .HasOne(pi => pi.Draft)
                .WithMany()
                .HasForeignKey(pi => pi.DraftId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PromptItem>()
                .HasIndex(pi => new { pi.PromptId, pi.Order })
                .IsUnique();

            modelBuilder.Entity<Question>()
                .HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Draft)
                .WithMany(d => d.Questions)
                .HasForeignKey(q => q.DraftId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Response>()
                .HasOne<Test>()
                .WithMany(t => t.Responses)
                .HasForeignKey(r => r.TestId);

            modelBuilder.Entity<Scratch>()
                .HasMany(s => s.Drafts)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TestQuestion>()
                .HasKey(tq => new { tq.TestId, tq.QuestionId });

            modelBuilder.Entity<Section>();

            modelBuilder.Entity<Shot>();

            modelBuilder.Entity<Test>()
                .HasMany(t => t.TestQuestions)
                .WithOne(tq => tq.Test)
                .HasForeignKey(tq => tq.TestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.TestQuestions)
                .WithOne(tq => tq.Question)
                .HasForeignKey(tq => tq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Test>()
                .HasMany(t => t.Questions)
                .WithOne(q => q.Test)
                .HasForeignKey(q => q.TestId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TestPerformance>()
                .HasMany(qp => qp.QuestionsPerformance)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AILog>? AILogs { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<BookTask>? BookTasks { get; set; }
        public DbSet<Chapter>? Chapters { get; set; }
        public DbSet<Content>? Contents { get; set; }
        public DbSet<Draft>? Drafts { get; set; }
        public DbSet<Option>? Options { get; set; }
        public DbSet<PDFData>? PDFData { get; set; }
        public DbSet<PDFDataPages>? PDFDataPages { get; set; }
        public DbSet<Prompt>? Prompts { get; set; }
        public DbSet<PromptItem>? PromptItems { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<Response>? Responses { get; set; }
        public DbSet<Scratch>? Scratches { get; set; }
        public DbSet<Section>? Sections { get; set; }
        public DbSet<Shot>? Shots { get; set; }
        public DbSet<Test>? Tests { get; set; }
        public DbSet<User>? Users { get; set; }

        public DbSet<TestPerformance>? TestsPerformance { get; set; }

        public DbSet<QuestionsPerformance>? QuestionsPerformance { get; set; }

        public DbSet<TestQuestion>? TestsQuestions { get; set; }

        public DbSet<Paid>? Paids { get; set; }
    }
}
