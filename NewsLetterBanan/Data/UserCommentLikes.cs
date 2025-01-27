using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{

    public class UserCommentLikes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary Key
        public string UserId { get; set; } // Foreign Key to User
        public int CommentId { get; set; } // Foreign Key to Comment

        // Navigation properties
        public virtual User User { get; set; }
        public virtual Comment Comment { get; set; }
    }


}
