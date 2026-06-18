using Dapper;
using NoteAppApi.DTOs.Note;
using NoteAppApi.Entities;
using NoteAppApi.Infrastructure.Persistence;

namespace NoteAppApi.Repositories;

public class NoteRepository
{
    private readonly DapperContext _context;

    public NoteRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<Note> CreateAsync(Note note)
    {
        const string sql = """
                               INSERT INTO notes (user_id, title, content)
                               OUTPUT INSERTED.*
                               VALUES (@UserId, @Title, @Content);
                           """;

        using var connection = _context.CreateConnection();

        var created = await connection.QuerySingleAsync<Note>(sql, note);

        return created;
    }
    
    public async Task<(IEnumerable<Note> Items, int TotalCount)> GetAllByUserIdAsync(Guid userId, GetAllNoteQueryDTO query)
    {
        var allowedSortColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["createdAt"] = "created_at",
            ["updatedAt"] = "updated_at",
            ["title"] = "title",
        };

        var sortColumn = "created_at";
        if (query.SortBy is not null && allowedSortColumns.TryGetValue(query.SortBy, out var mapped))
            sortColumn = mapped;

        var direction = query.SortDir?.Equals("asc", StringComparison.OrdinalIgnoreCase) == true ? "ASC" : "DESC";

        var where = "user_id = @UserId AND deleted_at IS NULL";

        if (!string.IsNullOrWhiteSpace(query.Search))
            where += " AND (title LIKE @Search OR content LIKE @Search)";

        var countSql = $"SELECT COUNT(*) FROM notes WHERE {where};";

        var dataSql = $"""
                          SELECT * FROM notes
                          WHERE {where}
                          ORDER BY {sortColumn} {direction}
                          OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                      """;

        using var connection = _context.CreateConnection();

        var searchParam = query.Search is not null ? $"%{query.Search}%" : null;

        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, new { UserId = userId, Search = searchParam });
        var items = await connection.QueryAsync<Note>(dataSql, new { UserId = userId, Search = searchParam, Offset = (query.Page - 1) * query.PageSize, PageSize = query.PageSize });

        return (items, totalCount);
    }

    public async Task<Note?> GetByIdAsync(Guid id)
    {
        const string sql = """
                               SELECT * FROM notes WHERE id = @Id AND deleted_at IS NULL;
                           """;

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Note>(sql, new { Id = id });
    }

    public async Task UpdateAsync(Guid id, string? title, string? content)
    {
        const string sql = """
                               UPDATE notes
                               SET
                                   title = COALESCE(@Title, title),
                                   content = COALESCE(@Content, content),
                                   updated_at = GETDATE()
                               WHERE id = @Id AND deleted_at IS NULL;
                           """;

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(sql, new { Id = id, Title = title, Content = content });
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        const string sql = """
                               UPDATE notes SET deleted_at = GETDATE() WHERE id = @Id AND deleted_at IS NULL;
                           """;

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(sql, new { Id = id });
    }
}
