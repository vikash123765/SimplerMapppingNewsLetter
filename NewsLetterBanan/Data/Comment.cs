
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{

    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        public required int ArticleId { get; set; }

        [Required]
        public required string UserId { get; set; } // UserId string(Guid) standard

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DateStamp { get; set; } = DateTime.Now;

        [MaxLength(300)]
        public required string Content { get; set; }

        public bool IsArchived { get; set; } = false;

        public virtual User User { get; set; }
        public virtual Article Article { get; set; }
        public virtual ICollection<UserCommentLikes> UserCommentLikes { get; set; } = new HashSet<UserCommentLikes>();
    }
}
