namespace NoteAppApi.DTOs.User;

public record UserResponseDTO(
    Guid Id,
    string FirstName,
    string LastName,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
