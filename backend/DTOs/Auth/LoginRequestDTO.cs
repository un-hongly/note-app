using System.ComponentModel.DataAnnotations;

namespace NoteAppApi.DTOs.Auth;

public class LoginRequestDTO
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}
