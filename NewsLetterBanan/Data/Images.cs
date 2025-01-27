using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        public int TagId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ImgPath { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [Required]
        [MaxLength(70)]
        public string AlternativeText { get; set; }

        [Required]
        [MaxLength(100)]
        public string ImgDescription { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ImgSourceURL { get; set; }

        [MaxLength(60)]
        public string License { get; set; }

        public virtual Tag Tag { get; set; }

    }
}
