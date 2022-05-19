using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleBlogNet.Database;
using SimpleBlogNet.Models;

namespace SimpleBlogNet.Pages;

public class AddPostModel : PageModel
{
    private readonly ILogger<AddPostModel> _logger;
    private readonly DatabaseRepository _databaseRepository;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public string Title { get; set; }

    [BindProperty]
    public string Content { get; set; }

    [BindProperty]
    public string Author { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string ImageURL { get; set; }

    public AddPostModel(ILogger<AddPostModel> logger, DatabaseRepository databaseRepository, IConfiguration configuration)
    {
        _logger = logger;
        _databaseRepository = databaseRepository;
        _configuration = configuration;
    }

    public async Task<IActionResult> OnPost()
    {
        if (Password != _configuration["PostPassword"] || string.IsNullOrWhiteSpace(Content) || string.IsNullOrWhiteSpace(Author) || string.IsNullOrWhiteSpace(Title))
            return RedirectToPage("AddPost");

        await _databaseRepository.AddPostAsync(new Post()
        {
            Author = Author,
            Content = Content,
            ImageUrl = ImageURL,
            PublishTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Title = Title
        });

        return RedirectToPage("Blog");
    }
}
