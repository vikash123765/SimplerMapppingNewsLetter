﻿using System.Reflection.Metadata;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return StatusCode(403, "User is not authenticated.");
                }

                // Create the Article
                var article = new Article
                {
                    Headline = model.Headline,
                    Content = model.Content,
                    ContentSummary = model.ContentSummary,
                    ImageUrl = model.ImageUrl,
                    DateStamp = model.DateStamp,
                    SourceURL = model.SourceURL,
                    IsArchived = false,
                    CommentsOnOff = true,
                    UserId = userId
                };

                // Handle Tag logic
                var existingTag = _context.Tags.FirstOrDefault(t => t.TagName == model.TagName);
                if (existingTag != null)
                {
                    // Use the existing tag
                    article.Tags.Add(existingTag);
                }
                else
                {
                    // Create a new tag
                    var newTag = new Tag
                    {
                        TagName = model.TagName,
                        TagDescription = model.TagDescription
                    };
                    _context.Tags.Add(newTag);
                    article.Tags.Add(newTag);
                }

            
                var existingCategory = _context.Categories.FirstOrDefault(c => c.Name == model.CategoryName);

               
                if (existingCategory != null)
                {
                    // Use the existing tag
                    article.Categories.Add(existingCategory);
                }
                else
                {
                    // Create a new category
                    var newCategory = new Category
                    {
                        Name = model.CategoryName,
                        Description = model.CategoryDescription
                    };
                    _context.Categories.Add(newCategory);
                    article.Categories.Add(newCategory);
                }

                // Save the article
                _context.Articles.Add(article);
                _context.SaveChanges();

                return RedirectToAction("ManageArticles");
            }

       
            return View(model);
        }



        // GET: /Admin/EditArticle/{id}
        [HttpGet("EditArticle/{id}")]
        public async Task<IActionResult> EditArticle(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Categories)
                .Include(a => a.Tags)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound(); // Return NotFound if the article doesn't exist
            }
             
            var viewModel = new CreateArticleViewModel
            {
               
                Headline = article.Headline,
                Content = article.Content,
                ContentSummary = article.ContentSummary,
                ImageUrl = article.ImageUrl,
                SourceURL = article.SourceURL,
                IsArchived = article.IsArchived,
                CommentsOnOff = article.CommentsOnOff,
                IsEditorsChoice = article.IsEditorsChoice,
                CategoryName = article.Categories.FirstOrDefault()?.Name, // Assuming the article has a primary category
                CategoryDescription = article.Categories.FirstOrDefault()?.Description, // Assuming the article has a primary category
                TagName = article.Tags.FirstOrDefault()?.TagName, // Assuming the article has at least one tag
                TagDescription = article.Tags.FirstOrDefault()?.TagDescription // Assuming the article has at least one tag
            };

            return View(viewModel); // Pass the view model to the view for editing
        }

        // POST: /Admin/EditArticle/{id}
        [HttpPost("EditArticle/{id}")]
        public async Task<IActionResult> EditArticle(int id, CreateArticleViewModel viewModel)
        {
         

            if (ModelState.IsValid)
            {
                var article = await _context.Articles
                    .Include(a => a.Categories)
                    .Include(a => a.Tags)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (article == null)
                {
                    return NotFound(); // Return NotFound if the article doesn't exist
                }

                // Update the article properties
                article.Headline = viewModel.Headline;
                article.Content = viewModel.Content;
                article.ContentSummary = viewModel.ContentSummary;
                article.ImageUrl = viewModel.ImageUrl;
                article.SourceURL = viewModel.SourceURL;
                article.IsArchived = viewModel.IsArchived;
                article.CommentsOnOff = viewModel.CommentsOnOff;
                article.IsEditorsChoice = viewModel.IsEditorsChoice;

                // Handle Category - if not already in database, create it
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == viewModel.CategoryName);

                if (existingCategory == null)
                {
                    var newCategory = new Category
                    {
                        Name = viewModel.CategoryName,
                        Description = viewModel.CategoryDescription
                    };
                  
                    article.Categories.Add(newCategory);
                }
                else
                {
                   
                    article.Categories.Add(existingCategory);
                }

                // Handle Tag - if not already in database, create it
                var existingTag = await _context.Tags
                    .FirstOrDefaultAsync(t => t.TagName == viewModel.TagName);

                if (existingTag == null)
                {
                    var newTag = new Tag
                    {
                        TagName = viewModel.TagName,
                        TagDescription = viewModel.TagDescription
                    };
                    // Remove the existing tags and add the new one
                 
                    article.Tags.Add(newTag);
                }
                else
                {
                   
                    article.Tags.Add(existingTag);
                }

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Flash success message (optional)
                TempData["SuccessMessage"] = "Article updated successfully.";

                return RedirectToAction("ManageArticles"); // Redirect to ManageArticles after saving
            }

            // If model state is invalid, redisplay the form with errors
            return View(viewModel);
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

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole( AssignRoleRequest request)
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
