using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DateStamp { get; set; } = DateTime.Now;

        [MaxLength(60)]
        public string Headline { get; set; } = string.Empty;

        [MaxLength(15000)]
        public string Content { get; set; } = string.Empty;

        [MaxLength(350)]
        public string ContentSummary { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string ImageUrl { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty; // The ID of the User who authored this article

        [Column(TypeName = "varchar(100)")]
        public string SourceURL { get; set; } = string.Empty;

        public bool IsArchived { get; set; } = false;
        public bool CommentsOnOff { get; set; } = false;
        public int Views { get; set; } = 0;
        public int Likes { get; set; } = 0;
        public bool IsEditorsChoice { get; set; }

        // Many-to-many relationship with Tag
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        // Many-to-many relationship with Category
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public virtual User User { get; set; } // The author of the article
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<UserLikes> UserLikes { get; set; } = new HashSet<UserLikes>();
    }

}