using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleBlogNet.Database;
using SimpleBlogNet.Models;

namespace SimpleBlogNet.Pages;
public class BlogModel : PageModel
{
    private readonly ILogger<BlogModel> _logger;
    private readonly DatabaseRepository _databaseRepository;

    [BindProperty(SupportsGet = true)]
    public int? Id { get; set; }

    public Post? Post { get; set; }
    public IEnumerable<Post> RecentPosts { get; set; }

    public BlogModel(ILogger<BlogModel> logger, DatabaseRepository databaseRepository)
    {
        _logger = logger;
        _databaseRepository = databaseRepository;
    }

    public async Task OnGet()
    {
        RecentPosts = await _databaseRepository.GetPostsAsync();

        if (Id == null)
            Post = await _databaseRepository.GetPostAsync(RecentPosts.FirstOrDefault().Id);
        else
            Post = await _databaseRepository.GetPostAsync(Id.Value);
    }
}
