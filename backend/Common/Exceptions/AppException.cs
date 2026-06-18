using System.Net;

namespace NoteAppApi.Common.Exceptions;

public class AppException : Exception
{
    public string ErrorCode { get; }
    public HttpStatusCode StatusCode { get; }

    public AppException(
        string errorCode,
        string message,
        HttpStatusCode statusCode
    ) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}