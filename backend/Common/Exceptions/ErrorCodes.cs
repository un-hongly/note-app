namespace NoteAppApi.Common.Exceptions;

public static class ErrorCodes
{
    public const string UsernameAlreadyExists = "USERNAME_ALREADY_EXISTS";
    public const string PasswordMismatch = "PASSWORD_MISMATCH";
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string NoteNotFound = "NOTE_NOT_FOUND";
    public const string Forbidden = "FORBIDDEN";
}