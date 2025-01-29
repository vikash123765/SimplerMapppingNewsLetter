using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsLetterBanan.Data;
using NewsLetterBanan.Services.Interfaces;

namespace NewsLetterBanan.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserController(IUserService userService, ApplicationDbContext db, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = db;
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Index method that returns the view with the action forms
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAction(string action, Dictionary<string, string> formData)
        {
            switch (action)
            {
                case "AddComment":
                    return await AddComment(new AddCommentRequest
                    {
                        ArticleId = int.Parse(formData["ArticleId"]),
                        UserId = formData["UserId"],
                        Content = formData["Content"]
                    });

                case "DeleteComment":
                    return await DeleteComment(new DeleteCommentRequest
                    {
                        CommentId = int.Parse(formData["CommentId"]),
                        UserId = formData["UserId"]
                    });

                case "EditComment":
                    return await EditComment(new EditCommentRequest
                    {
                        CommentId = int.Parse(formData["CommentId"]),
                        UserId = formData["UserId"],
                        Content = formData["Content"]
                    });
                case "LikeArticle":
                    return await LikeArticle(new LikeArticleRequest
                    {
                        ArticleId = int.Parse(formData["ArticleId"]),
                        UserId = formData["UserId"]
                    });

                case "UnlikeArticle":
                    return await UnlikeArticle(new UnlikeArticleRequest
                    {
                        ArticleId = int.Parse(formData["ArticleId"]),
                        UserId = formData["UserId"]
                    });
                case "LikeComment":
                    return await LikeComment(new LikeCommentRequest
                    {
                        UserId = formData["UserId"],
                        CommentId = int.Parse(formData["CommentId"])
                    });

                // Handling the UnlikeComment action
                case "UnlikeComment":
                    return await UnlikeComment(new UnlikeCommentRequest
                    {
                        UserId = formData["UserId"],
                        CommentId = int.Parse(formData["CommentId"])
                    });

                // Handling the IsCommentLiked action
                case "IsCommentLiked":
                    return await IsCommentLiked(new IsCommentLikedRequest
                    {
                        UserId = formData["UserId"],
                        CommentId = int.Parse(formData["CommentId"])
                    });


                default:
                    return BadRequest("Invalid action.");
            }
        }

        // Add Comment method
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(AddCommentRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var article = await _dbContext.Articles.FindAsync(request.ArticleId);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            var comment = new Comment
            {
                ArticleId = request.ArticleId,
                UserId = request.UserId,
                Content = request.Content,
                DateStamp = DateTime.UtcNow
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            TempData["Message"] = "Comment added successfully!";
            return RedirectToAction("Index");
        }

        // Delete Comment method
        [HttpPost("DeleteComment")]
        public async Task<IActionResult> DeleteComment(DeleteCommentRequest request)
        {
            var comment = await _dbContext.Comments.FindAsync(request.CommentId);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            if (comment.UserId != request.UserId)
            {
                return BadRequest("You can only delete your own comments.");
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

            TempData["Message"] = "Comment deleted successfully!";
            return RedirectToAction("Index");
        }

        // Edit Comment method
        [HttpPost("EditComment")]
        public async Task<IActionResult> EditComment(EditCommentRequest request)
        {
            var comment = await _dbContext.Comments.FindAsync(request.CommentId);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            if (comment.UserId != request.UserId)
            {
                return BadRequest("You can only edit your own comments.");
            }

            comment.Content = request.Content;
            comment.DateStamp = DateTime.UtcNow;

            _dbContext.Comments.Update(comment);
            await _dbContext.SaveChangesAsync();

            TempData["Message"] = "Comment updated successfully!";
            return RedirectToAction("Index");
        }


        // Request models for actions
        public class AddCommentRequest
        {
            public int ArticleId { get; set; }
            public string UserId { get; set; }
            public string Content { get; set; }
        }

        public class DeleteCommentRequest
        {
            public int CommentId { get; set; }
            public string UserId { get; set; }
        }

        public class EditCommentRequest
        {
            public int CommentId { get; set; }
            public string UserId { get; set; }
            public string Content { get; set; }
        }

        [HttpPost("LikeArticle")]
        public async Task<IActionResult> LikeArticle(LikeArticleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var article = await _dbContext.Articles.FindAsync(request.ArticleId);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            var existingLike = _dbContext.UserLikes
                .FirstOrDefault(x => x.UserId == request.UserId && x.ArticleId == request.ArticleId);
            if (existingLike != null)
            {
                return BadRequest("You already liked this article.");
            }

            var like = new UserLikes
            {
                ArticleId = request.ArticleId,
                UserId = request.UserId
            };

            _dbContext.UserLikes.Add(like);
            await _dbContext.SaveChangesAsync();

            return Ok("Article liked successfully.");
        }


        public class LikeArticleRequest
        {
            public int ArticleId { get; set; }
            public string UserId { get; set; }
        }

        // Unlike Article method
        [HttpPost("UnlikeArticle")]
        public async Task<IActionResult> UnlikeArticle(UnlikeArticleRequest request)
        {
            // Find the user by UserId
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Find the article by ArticleId
            var article = await _dbContext.Articles.FindAsync(request.ArticleId);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            // Find the like entry in the UserLikes table for the specific user and article
            var likeToRemove = await _dbContext.UserLikes
                .FirstOrDefaultAsync(ul => ul.UserId == request.UserId && ul.ArticleId == request.ArticleId);

            // If the like doesn't exist, return an error
            if (likeToRemove == null)
            {
                return NotFound("Like not found.");
            }

            // Remove the like record from UserLikes table
            _dbContext.UserLikes.Remove(likeToRemove);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Set a success message to TempData
            TempData["Message"] = "Article unliked successfully!";

            // Redirect the user back to the index (or another relevant page)
            return RedirectToAction("Index");
        }



        public class UnlikeArticleRequest
        {
            public int ArticleId { get; set; }
            public string UserId { get; set; }
        }
        //[HttpGet("MyPage")]
        //    public async Task<IActionResult> MyPage(string userId)
        //    {
        //        var user = await _userManager.FindByIdAsync(userId);
        //        if (user == null)
        //        {
        //            return NotFound("User not found.");
        //        }


        //        var userComments = _dbContext.Comments
        //            .Where(c => c.UserId == userId)
        //            .ToList();

        //        var userCommentsLikes = _dbContext.Comments
        //          .Where(c => c.UserId == userId)
        //          .ToList();

        //        var subscriptions = _dbContext.Subscriptions
        //            .Where(s => s.UserId == userId)
        //            .ToList();

        //        return Ok(new
        //        {
        //            UserDetails = new { user.UserName, user.Email },
        //            Comments = userComments,
        //            Subscriptions = subscriptions
        //        });
        //    }


        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe(string userId, string subscriptionType, decimal price, int durationInDays)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Create a new subscription
            var subscription = new Subscription
            {
                UserId = userId,
                SubscriptionType = subscriptionType,
                Price = price,
                Expires = DateTime.UtcNow.AddDays(durationInDays), // Set expiration date based on duration
                PaymentComplete = true // Assuming payment was completed
            };

            _dbContext.Subscriptions.Add(subscription);
            await _dbContext.SaveChangesAsync();

            return Ok($"User {user.UserName} subscribed to {subscriptionType}.");
        }

        [HttpPost("Unsubscribe")]
        public async Task<IActionResult> Unsubscribe(string userId, int subscriptionId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.Id == subscriptionId && s.UserId == userId);

            if (subscription == null)
            {
                return NotFound("Subscription not found.");
            }

            _dbContext.Subscriptions.Remove(subscription);
            await _dbContext.SaveChangesAsync();

            return Ok("Unsubscribed successfully.");
        }

        [HttpGet("GetActiveSubscriptions")]
        public async Task<IActionResult> GetActiveSubscriptions(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var activeSubscriptions = await _dbContext.Subscriptions
                .Where(s => s.UserId == userId && s.Expires > DateTime.UtcNow) // Active if not expired
                .ToListAsync();

            return Ok(activeSubscriptions);
        }

        // Like Comment
        [HttpPost("LikeComment")]
        public async Task<IActionResult> LikeComment(LikeCommentRequest request)
        {
          var  userId = request.UserId;

            var commentId = request.CommentId;
            // Check if the user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the comment exists
            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound("Comment not found.");
            }

            // Check if the like already exists
            var existingLike = await _dbContext.UserCommentLikes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.CommentId == commentId);

            if (existingLike != null)
            {
                return BadRequest("Comment already liked by user.");
            }

            // Add a new like
            var like = new UserCommentLikes
            {
                UserId = userId,
                CommentId = commentId
            };

            _dbContext.UserCommentLikes.Add(like);
            await _dbContext.SaveChangesAsync();

            return Ok("Comment liked successfully.");
        }

        // Unlike Comment
        [HttpPost("UnlikeComment")]
        public async Task<IActionResult> UnlikeComment(UnlikeCommentRequest request)
        {
            var userId = request.UserId;

            var commentId = request.CommentId;
            // Check if the like exists
            var like = await _dbContext.UserCommentLikes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.CommentId == commentId);

            if (like == null)
            {
                return NotFound("Like not found.");
            }

            // Remove the like
            _dbContext.UserCommentLikes.Remove(like);
            await _dbContext.SaveChangesAsync();

            return Ok("Comment unliked successfully.");
        }

        // Check if Comment is Liked
        [HttpGet("IsCommentLiked")]
        public async Task<IActionResult> IsCommentLiked(IsCommentLikedRequest request)
        {
            var userId = request.UserId;

            var commentId = request.CommentId;
            // Check if the like exists
            var isLiked = await _dbContext.UserCommentLikes
                .AnyAsync(l => l.UserId == userId && l.CommentId == commentId);

            return Ok(isLiked);
        }
        public class LikeCommentRequest
        {
            public string UserId { get; set; }
            public int CommentId { get; set; }
        }
        public class UnlikeCommentRequest
        {
            public string UserId { get; set; }
            public int CommentId { get; set; }
        }

        public class IsCommentLikedRequest
        {
            public string UserId { get; set; }
            public int CommentId { get; set; }
        }



        [HttpGet("GetCommentLikes")]
        public async Task<IActionResult> GetCommentLikes(int commentId)
        {
            // Get all likes for the comment
            var likes = await _dbContext.UserCommentLikes
                .Where(l => l.CommentId == commentId)
                .Select(l => new { l.UserId, l.User.UserName })
                .ToListAsync();

            return Ok(likes);
        }









    }

}