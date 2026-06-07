using QuizMaster.Contracts.Auth;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IAuthService
    {
        Task<User> GetUser(int id);
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default);
    }
}
