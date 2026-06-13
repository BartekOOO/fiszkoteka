using QuizMaster.Contracts.Commands.Flashcards;
using QuizMaster.Wpf.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Delegates
{
    public delegate bool CreateFlashcardHandler(object sender, CreateFlashcardCommand command, int flashcardSetId);
}
