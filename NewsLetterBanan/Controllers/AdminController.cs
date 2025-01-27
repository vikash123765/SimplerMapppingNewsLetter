using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsLetterBanan.Data;
using NewsLetterBanan.Models;
using NewsLetterBanan.Services.Interfaces;

namespace NewsLetterBanan.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
       private readonly IArticleService _articleService;
     //   private readonly IUserService _userSevice;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, IArticleService articleService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _articleService = articleService;
            _userManager = userManager;
            _roleManager = roleManager;
          //  _userSevice = userSevice;
        }
        public IActionResult Index()
        {
            return View("AdminDashboard");
        }
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("/Admin/ManageArticles")] // Explicit route definition
        public async Task<IActionResult> ManageArticles()
        {
            var articles = await _context.Articles.ToListAsync(); // Fetch all articles
            return View(articles); // Pass articles to the view
        }

        [HttpGet("CreateArticle")]
        public IActionResult CreateArticle()
        {
            try
            {
                Console.WriteLine("Preparing CreateArticle form.");

                var viewModel = new CreateArticleViewModel
                {
                    DateStamp = DateTime.Now // Default DateStamp
                };

                Console.WriteLine("Form initialized successfully.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GET CreateArticle: {ex.Message}");
                return StatusCode(500, "An error occurred while loading the form.");
            }
        }



        [HttpPost("CreateArticle")]
        public IActionResult CreateArticle(CreateArticleViewModel model)
        {
            try
            {
                Console.WriteLine("Received form submission for CreateArticle.");

                if (ModelState.IsValid)
                {
                    Console.WriteLine("Model state is valid, proceeding with saving article.");

                    // Get current user's ID
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return StatusCode(403, "User is not logged in or authenticated.");
                    }

                    // Create and save the Article
                    var article = new Article
                    {
                        Headline = model.Headline,
                        Content = model.Content,
                        ContentSummary = model.ContentSummary,
                        ImageUrl = model.ImageUrl,
                        DateStamp = model.DateStamp == default ? DateTime.Now : model.DateStamp,
                        Category = model.Category,
                        SourceURL = model.SourceURL,
                        IsArchived = model.IsArchived,
                        CommentsOnOff = model.CommentsOnOff,
                        UserId = userId
                    };

                    _context.Articles.Add(article);
                    _context.SaveChanges();

                    // Create and save the Tag
                    var tag = new Tag
                    {
                        TagName = model.TagName,
                        TagDescription = model.TagDescription
                    };

                    _context.Tags.Add(tag);
                    _context.SaveChanges();

                    // Associate the tag with the article
                    article.Tags.Add(tag);
                    _context.SaveChanges();

                    return RedirectToAction("ManageArticles");
                }
                else
                {
                    Console.WriteLine("Model validation failed.");
                    return View(model); // Return form with validation errors
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in POST CreateArticle: {ex.Message}");
                return StatusCode(500, "An error occurred while saving the article.");
            }
        }



        // GET: /Admin/EditArticle/{id}
        [HttpGet("EditArticle/{id}")]
        public async Task<IActionResult> EditArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound(); // Return NotFound if the article doesn't exist
            }

            return View(article); // Pass the article to the view for editing
        }

        // POST: /Admin/EditArticle/{id}
        [HttpPost("EditArticle/{id}")]
        public async Task<IActionResult> EditArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return NotFound(); // Ensure the article ID matches
            }

            if (ModelState.IsValid)
            {
                _context.Articles.Update(article); // Update the article
                await _context.SaveChangesAsync();

                // Flash success message (optional)
                TempData["SuccessMessage"] = "Article updated successfully.";

                return RedirectToAction("ManageArticles"); // Redirect to ManageArticles after saving
            }

            // If model state is invalid, redisplay the form with errors
            return View(article);
        }


        // POST: /Admin/DeleteArticle/{id}
        [HttpPost("DeleteArticle/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound(); // Return NotFound if the article doesn't exist
            }

            _context.Articles.Remove(article); // Remove the article from the database
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageArticles"); // Redirect back to ManageArticles
        }

        [HttpGet("ManageUsers")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        [HttpGet("CreateUser")]
        public IActionResult CreateUser()
        {
            return View(new User());
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return View(user);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageUsers");
        }

        [HttpGet("EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("EditUser/{id}")]
        public async Task<IActionResult> EditUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;

            await _context.SaveChangesAsync();
            return RedirectToAction("ManageUsers");
        }

        [HttpPost("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageUsers");
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roleExist = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(request.RoleName));
            }

            var result = await _userManager.AddToRoleAsync(user, request.RoleName);
            if (result.Succeeded)
            {
                return Ok($"User {user.UserName} has been assigned the {request.RoleName} role.");
            }

            return BadRequest("Error assigning role.");
        }


        public class AssignRoleRequest
        {
            public string UserId { get; set; }
            public string RoleName { get; set; }
        }
    }
}
