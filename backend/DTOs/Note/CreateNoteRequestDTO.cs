using System.ComponentModel.DataAnnotations;

namespace NoteAppApi.DTOs.Note;

public record CreateNoteRequestDTO(
    [Required]
    string Title,
    string? Content
    );
