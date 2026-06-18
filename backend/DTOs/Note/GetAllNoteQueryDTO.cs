using NoteAppApi.DTOs.Common;

namespace NoteAppApi.DTOs.Note;

public class GetAllNoteQueryDTO : PaginateQuery
{
    public string? Search { get; set; }
}
