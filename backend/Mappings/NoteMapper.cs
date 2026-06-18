using NoteAppApi.DTOs.Note;
using NoteAppApi.Entities;
using Riok.Mapperly.Abstractions;

namespace NoteAppApi.Mappings;

[Mapper]
public static partial class NoteMapper
{
    public static partial NoteResponseDTO ToResponse(Note note);
}
