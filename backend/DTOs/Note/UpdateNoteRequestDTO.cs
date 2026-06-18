namespace NoteAppApi.DTOs.Note;

public record UpdateNoteRequestDTO(
    string? Title,
    string? Content
);
