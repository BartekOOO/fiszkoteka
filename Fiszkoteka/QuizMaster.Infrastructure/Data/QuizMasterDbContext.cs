using Microsoft.EntityFrameworkCore;
using QuizMaster.Application.Interfaces;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Contracts.Models;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Infrastructure.Data
{
    public class QuizMasterDbContext : DbContext, IQuizMasterDbContext
    {
        public QuizMasterDbContext(DbContextOptions<QuizMasterDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FlashcardSet> FlashcardSets { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<UserFlashcardProgress> UserFlashcardProgresses { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<LearningSession> LearningSessions { get; set; }
        public DbSet<LearningSessionItem> LearningSessionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.UserName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<FlashcardSet>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(x => x.Description)
                    .HasMaxLength(1000);

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.FlashcardSets)
                    .HasForeignKey(x => x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.FlashcardSets)
                    .HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<Flashcard>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Question)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(x => x.Answer)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasOne(x => x.FlashcardSet)
                    .WithMany(x => x.Flashcards)
                    .HasForeignKey(x => x.FlashcardSetId);
            });

            modelBuilder.Entity<UserFlashcardProgress>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => new { x.UserId, x.FlashcardId })
                    .IsUnique();

                entity.HasOne(x => x.User)
                    .WithMany(x => x.FlashcardProgresses)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.Flashcard)
                    .WithMany(x => x.Progresses)
                    .HasForeignKey(x => x.FlashcardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RevokedToken>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Jti)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.RevokedAt)
                    .IsRequired();

                entity.Property(x => x.ExpiresAt)
                    .IsRequired();

                entity.HasIndex(x => x.Jti)
                    .IsUnique();

                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<LearningSession>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.StartedAt)
                    .IsRequired();

                entity.Property(x => x.TotalFlashcardsCount)
                    .IsRequired();

                entity.Property(x => x.ReviewedFlashcardsCount)
                    .IsRequired();

                entity.Property(x => x.CorrectAnswersCount)
                    .IsRequired();

                entity.Property(x => x.WrongAnswersCount)
                    .IsRequired();

                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.FlashcardSet)
                    .WithMany()
                    .HasForeignKey(x => x.FlashcardSetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LearningSessionItem>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.IsAnswered)
                    .IsRequired();

                entity.HasOne(x => x.LearningSession)
                    .WithMany()
                    .HasForeignKey(x => x.LearningSessionId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Flashcard)
                    .WithMany()
                    .HasForeignKey(x => x.FlashcardId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new { x.LearningSessionId, x.FlashcardId })
                    .IsUnique();
            });
        }
    }
}
