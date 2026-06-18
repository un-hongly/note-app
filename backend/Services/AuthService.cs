using System.Net;
using NoteAppApi.Common.Exceptions;
using NoteAppApi.DTOs;
using NoteAppApi.DTOs.Auth;
using NoteAppApi.DTOs.User;
using NoteAppApi.Entities;
using NoteAppApi.Infrastructure.Configuration;
using NoteAppApi.Infrastructure.Security;
using NoteAppApi.Mappings;
using NoteAppApi.Repositories;

namespace NoteAppApi.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly TokenService _tokenService;

    public AuthService(UserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<UserResponseDTO> RegisterAsync(RegisterRequestDTO request)
    {
        if (request.Password != request.PasswordConfirm)
            throw new AppException(ErrorCodes.PasswordMismatch, "Passwords do not match", HttpStatusCode.BadRequest);

        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);

        if (existingUser != null)
            throw new AppException(ErrorCodes.UsernameAlreadyExists, "Username already exists", HttpStatusCode.BadRequest);

        var user = await _userRepository.CreateAsync( new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
        });

        return UserMapper.ToResponse(user);
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user == null) throw new AppException(ErrorCodes.InvalidCredentials, "Invalid credentials", HttpStatusCode.Unauthorized);

        var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);

        if (!isValid) throw new AppException(ErrorCodes.InvalidCredentials, "Invalid credentials", HttpStatusCode.Unauthorized);

        var accessToken = _tokenService.CreateToken(user);
        return new LoginResponseDTO(accessToken);
    }
    
}
