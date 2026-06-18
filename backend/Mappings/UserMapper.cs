using NoteAppApi.DTOs.User;
using NoteAppApi.Entities;
using Riok.Mapperly.Abstractions;

namespace NoteAppApi.Mappings;

[Mapper]
public static partial class UserMapper
{
    public static partial UserResponseDTO ToResponse(User user);
}
