using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBlogNet.Models;

public class Post
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public long PublishTimestamp { get; set; }

    public string Author { get; set; }

    public string ImageUrl { get; set; }
}