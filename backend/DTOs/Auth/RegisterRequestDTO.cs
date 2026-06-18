using System.ComponentModel.DataAnnotations;

namespace NoteAppApi.DTOs.Auth;

public class RegisterRequestDTO
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }

    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }

    [Required]
    public string PasswordConfirm { get; set; }
}
