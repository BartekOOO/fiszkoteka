using QuizMaster.Contracts.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.ViewModels
{
    public sealed class PublicFlashcardSetListItemViewModel
    {
        public int Id { get; }

        public string Name { get; }
        public string Description { get; }

        public string CategoryName { get; }
        public string UserName { get; }

        public int FlashcardsCount { get; }

        public DateTime CreatedAt { get; }

        public string FlashcardsText => $"{FlashcardsCount} fiszek";

        public string CategoryText => string.IsNullOrWhiteSpace(CategoryName)
            ? "Bez kategorii"
            : CategoryName;

        public string AuthorText => string.IsNullOrWhiteSpace(UserName)
            ? "Autor: nieznany"
            : $"Autor: {UserName}";

        public string CreatedAtText => $"Utworzono {CreatedAt:dd.MM.yyyy}";

        public PublicFlashcardSetListItemViewModel(FlashcardSetListItemDto set)
        {
            Id = set.Id;

            Name = string.IsNullOrWhiteSpace(set.Name)
                ? "Bez nazwy"
                : set.Name;

            Description = string.IsNullOrWhiteSpace(set.Description)
                ? "Brak opisu"
                : set.Description;

            CategoryName = set.CategoryName;
            UserName = set.Author;

            FlashcardsCount = set.FlashcardsCount;
            CreatedAt = set.CreatedAt;
        }
    }
}
