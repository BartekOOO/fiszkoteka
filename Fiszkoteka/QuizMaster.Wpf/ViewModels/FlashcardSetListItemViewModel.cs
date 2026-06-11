using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.ViewModels
{
    public sealed class FlashcardSetListItemViewModel
    {
        public int Id { get; }

        public string Name { get; }
        public string Description { get; }

        public string CategoryName { get; }

        public int FlashcardsCount { get; }

        public bool IsPublic { get; }

        public DateTime CreatedAt { get; }

        public string VisibilityText => IsPublic
            ? "Publiczny"
            : "Prywatny";

        public string FlashcardsText => $"{FlashcardsCount} fiszek";

        public string CategoryText => string.IsNullOrWhiteSpace(CategoryName)
            ? "Bez kategorii"
            : CategoryName;

        public string CreatedAtText => $"Utworzono {CreatedAt:dd.MM.yyyy}";

        public FlashcardSetListItemViewModel(FlashcardSet set)
        {
            Id = set.Id;
            Name = string.IsNullOrWhiteSpace(set.Name)
                ? "Bez nazwy"
                : set.Name;

            Description = string.IsNullOrWhiteSpace(set.Description)
                ? "Brak opisu"
                : set.Description;

            CategoryName = set.Category?.Name;

            FlashcardsCount = set.Flashcards?.Count ?? 0;

            IsPublic = set.IsPublic;
            CreatedAt = set.CreatedAt;
        }
    }
}
