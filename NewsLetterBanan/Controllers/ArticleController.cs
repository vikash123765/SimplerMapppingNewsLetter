using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsLetterBanan.Data;
using NewsLetterBanan.Services.Interfaces;

namespace NewsLetterBanan.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IArticleService _articleService;
        public ArticleController(ApplicationDbContext context, IArticleService articleService)
        {
            _context = context;
            _articleService = articleService;
        }


        [HttpGet("GetAllArticles")]
        // GetAllArticles Action
        public async Task<IActionResult> GetAllArticles()
        {
            // Option 1: Using DbContext directly (if you want to get all articles)
            var allArticles = await _context.Articles
                 .Include(a => a.Categories)
                .OrderByDescending(a => a.DateStamp) // Optional: order by PublishedDate
                .ToListAsync();

            if (allArticles == null || !allArticles.Any())
            {
                ViewBag.Message = "No articles found.";
                return View();
            }

            return View(allArticles); // Passing articles to the View
        }
    }
}
