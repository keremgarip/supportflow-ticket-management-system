using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SupportFlow.Api.Data;
using SupportFlow.Api.DTOs.Auth;
using SupportFlow.Api.Helpers;
using SupportFlow.Api.Interfaces;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(
        AppDbContext context,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService,
        IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<AuthResponseDto> RegisterAsync(
        RegisterDto dto,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(dto.Email);

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = normalizedEmail,
            Role = "Customer",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(
            _jwtSettings.ExpirationMinutes);

        var token = _tokenService.CreateToken(
            user,
            expiresAt);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = MapToAuthUserDto(user)
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(
    LoginDto dto,
    CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(dto.Email);

        var user = await _context.Users
            .SingleOrDefaultAsync(
                user => user.Email == normalizedEmail,
                cancellationToken);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var verificationResult =
            _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

        if (verificationResult ==
            PasswordVerificationResult.Failed)
        {
            return null;
        }

        if (verificationResult ==
            PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    dto.Password);

            await _context.SaveChangesAsync(
                cancellationToken);
        }

        var expiresAt = DateTime.UtcNow.AddMinutes(
            _jwtSettings.ExpirationMinutes);

        var token = _tokenService.CreateToken(
            user,
            expiresAt);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = expiresAt,
            User = MapToAuthUserDto(user)
        };
    }

    public async Task<AuthUserDto?> GetCurrentUserAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => new AuthUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(email);

        return await _context.Users
            .AsNoTracking()
            .AnyAsync(
                user => user.Email == normalizedEmail,
                cancellationToken);
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static AuthUserDto MapToAuthUserDto(User user)
    {
        return new AuthUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}