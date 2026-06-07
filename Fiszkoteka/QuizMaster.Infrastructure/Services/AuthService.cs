using Microsoft.EntityFrameworkCore;
using QuizMaster.Contracts.Auth;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Interfaces;
using QuizMaster.Contracts.Models;
using QuizMaster.Core.Models;
using QuizMaster.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
                throw new UserAlreadyExistsException(request.UserName);

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
                throw new InvalidLoginException();

            var passwordValid = _passwordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash);

            if (!passwordValid)
                throw new InvalidLoginException();

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Token = token
            };
        }

        public async Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(request.Token))
                return;

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(request.Token);

            var jti = jwt.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)
                ?.Value;

            var userIdText = jwt.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
                ?.Value;

            if (string.IsNullOrWhiteSpace(jti))
                return;

            int.TryParse(userIdText, out var userId);

            var alreadyRevoked = await _context.RevokedTokens
                .AnyAsync(x => x.Jti == jti, cancellationToken);

            if (alreadyRevoked)
                return;

            _context.RevokedTokens.Add(new RevokedToken
            {
                Jti = jti,
                UserId = userId,
                RevokedAt = DateTime.UtcNow,
                ExpiresAt = jwt.ValidTo
            });

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
