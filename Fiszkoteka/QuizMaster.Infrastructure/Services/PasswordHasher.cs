using QuizMaster.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Infrastructure.Services
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
