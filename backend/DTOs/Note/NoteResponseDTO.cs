namespace NoteAppApi.DTOs.Note;

public record NoteResponseDTO(
    Guid Id,
    string Title,
    string? Content,
    Guid UserId,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );
