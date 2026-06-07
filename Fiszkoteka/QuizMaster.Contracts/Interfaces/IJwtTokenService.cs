using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
