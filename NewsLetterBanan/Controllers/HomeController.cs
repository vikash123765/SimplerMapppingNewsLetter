using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsLetterBanan.Data;
using NewsLetterBanan.Models;
using NewsLetterBanan.Services.Interfaces;

namespace NewsLetterBanan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        private readonly IUserService _userService;

        //  private readonly IWeatherService _weatherService;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
            //   _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            var latestArticles = await _context.Articles.OrderByDescending(a => a.DateStamp).Take(5).ToListAsync();
            var editorsChoice = await _context.Articles.Where(a => a.IsEditorsChoice).OrderByDescending(a => a.DateStamp).Take(3).ToListAsync();

            var viewModel = new NewsLetterBanan.Models.ViewModels.HomePageViewModel
            {
                Latest = latestArticles,
                EditorsChoice = editorsChoice
            };


            return View(viewModel);  // This will return the Index view
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
