using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
