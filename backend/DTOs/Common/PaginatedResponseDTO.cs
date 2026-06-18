namespace NoteAppApi.DTOs.Common;

public record PaginatedResponseDTO<T>(
    List<T> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);
