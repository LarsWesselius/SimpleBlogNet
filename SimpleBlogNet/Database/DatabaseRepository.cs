using Dapper;
using SimpleBlogNet.Models;
using System.Data.Common;

namespace SimpleBlogNet.Database;

public class DatabaseRepository
{
    private readonly DbConnection _connection;

    public DatabaseRepository(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<Post?> GetLatestPostAsync(CancellationToken cancellationToken = default)
    {
        return await _connection.QueryFirstOrDefaultAsync<Post>("SELECT id, title, content, publish_time AS PublishTimestamp, author, image_url AS ImageUrl FROM posts ORDER BY publish_time DESC LIMIT 1");
    }

    public async Task AddPostAsync(Post post, CancellationToken cancellationToken = default)
    {
        await _connection.ExecuteAsync("INSERT INTO posts (title, content, author, image_url, publish_time) VALUES(@Title, @Content, @Author, @ImageUrl, @PublishTimestamp)", post);
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(int count = 20, CancellationToken cancellationToken = default)
    {
        return await _connection.QueryAsync<Post>($"SELECT id, title, publish_time AS PublishTimestamp FROM posts ORDER BY publish_time DESC LIMIT {count}");
    }

    public async Task<Post?> GetPostAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _connection.QuerySingleOrDefaultAsync<Post>($"SELECT id, title, content, publish_time AS PublishTimestamp, author, image_url AS ImageUrl FROM posts WHERE id = {id}");
    }
}
