using QuizMaster.Contracts.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    }
}
