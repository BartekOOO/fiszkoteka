using QuizMaster.Contracts.Auth;
using QuizMaster.Wpf.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Interfaces
{
    public interface IAuthApiClient
    {
        Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task Logout(LogoutRequest request, CancellationToken cancellationToken = default);
    }
}
