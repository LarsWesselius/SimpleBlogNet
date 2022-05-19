using Dapper;
using System.Data.Common;

namespace SimpleBlogNet.Database;

public class DatabaseBootstrap
{
    private readonly DbConnection _connection;

    public DatabaseBootstrap(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task VerifyAndBootstrapIfNecessaryAsync()
    {
        var table = await _connection.QueryAsync<string>("SELECT name FROM sqlite_master WHERE type='table' AND name = 'posts'");
        if (string.IsNullOrEmpty(table.FirstOrDefault()))
            await BootstrapAsync();

        var ensureAtleastOnePost = await _connection.QuerySingleOrDefaultAsync<int>("SELECT COUNT(*) FROM posts");
        if (ensureAtleastOnePost == 0)
            await InsertFakePostAsync();
    }

    private async Task InsertFakePostAsync()
    {
        await _connection.ExecuteAsync($"INSERT INTO posts (title, content, publish_time, author, image_url) VALUES ('Simple Blog — Programming example in different languages', 'Just a placeholder for a *proper* post.', '{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}', 'Lars', 'https://picsum.photos/seed/simple/900/400')");
    }

    private async Task BootstrapAsync()
    {
        await _connection.ExecuteAsync("CREATE TABLE posts (" +
            "id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "title NVARCHAR(255) NOT NULL," +
            "publish_time INT NOT NULL," +
            "content TEXT NULL," +
            "author NVARCHAR(255)," +
            "image_url NVARCHAR(255)" +
            ");");
    }
}
