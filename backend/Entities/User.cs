namespace NoteAppApi.Entities;

public class User : BaseEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
