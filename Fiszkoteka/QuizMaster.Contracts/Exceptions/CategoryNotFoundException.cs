using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class CategoryNotFoundException : QuizMasterException
    {
        public CategoryNotFoundException(int id)
            : base($"Nie znaleziono kategorii o identyfikatorze {id}.", 404)
        {
        }

        private CategoryNotFoundException(string message)
            : base(message, 404)
        {
        }

        public static CategoryNotFoundException FromMessage(string message)
        {
            return new CategoryNotFoundException(
                string.IsNullOrWhiteSpace(message)
                    ? "Nie znaleziono kategorii."
                    : message);
        }
    }
}
