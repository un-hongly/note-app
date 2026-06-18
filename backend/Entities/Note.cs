
namespace NoteAppApi.Entities;

public class Note : BaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public Guid UserId { get; set; }
}