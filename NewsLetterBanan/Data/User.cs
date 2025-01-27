using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{
    public class User : IdentityUser
    {

        [MaxLength(50)] // string default is nvarchar(max)
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        public bool Newsletter { get; set; } = false;

        [MaxLength(23)]
        public string City { get; set; } = string.Empty;

        [MaxLength(69)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(40)]
        public string StripeKey { get; set; } = string.Empty;
        public bool AllowComment { get; set; } = true;

        public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
        public virtual ICollection<UserLikes> UserLikes { get; set; } = new HashSet<UserLikes>();

        public virtual ICollection<UserCommentLikes> UserCommentLikes { get; set; } = new HashSet<UserCommentLikes>();
    }
}
