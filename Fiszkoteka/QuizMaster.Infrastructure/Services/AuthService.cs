using Microsoft.EntityFrameworkCore;
using QuizMaster.Contracts.Auth;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Core.Models;
using QuizMaster.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Infrastructure.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly QuizMasterDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(
            QuizMasterDbContext context,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailExists)
                throw new Exception("Użytkownik z takim adresem email już istnieje.");

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user == null)
                throw new Exception("Nieprawidłowy email lub hasło.");

            var passwordValid = _passwordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash);

            if (!passwordValid)
                throw new Exception("Nieprawidłowy email lub hasło.");

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }
    }
}
