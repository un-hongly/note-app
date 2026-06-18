using Dapper;
using NoteAppApi.Entities;
using NoteAppApi.Infrastructure.Persistence;

namespace NoteAppApi.Repositories;

public class UserRepository
{
    private readonly DapperContext _context;
    
    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = """
                           SELECT * 
                           FROM users
                           WHERE Username = @Username
                           """;
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }
    
    public async Task<User> CreateAsync(User user)
    {
        const string sql = """
                               INSERT INTO users (first_name, last_name, username, password)
                               OUTPUT INSERTED.*
                               VALUES (@FirstName, @LastName, @Username, @Password);
                           """;

        using var connection = _context.CreateConnection();

        var created = await connection.QuerySingleAsync<User>(sql, user);

        return created;
    }

}