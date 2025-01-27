using NewsLetterBanan.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserLikes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Primary key

    [Required]
    public string UserId { get; set; } // Foreign key for User

    [Required]
    public int ArticleId { get; set; } // Foreign key for Article

    // Navigation properties
    public virtual User User { get; set; } // Navigation property for User
    public virtual Article Article { get; set; } // Navigation property for Article
}
