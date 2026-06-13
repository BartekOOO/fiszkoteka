using QuizMaster.Contracts.Commands.Flashcards;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Delegates
{
    public delegate Task<bool> EditFlashcardHandler(object sender, UpdateFlashcardCommand command, int id);
}
