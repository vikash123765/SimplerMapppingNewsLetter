using System.ComponentModel.DataAnnotations;

namespace NewsLetterBanan.Models.ViewModels
{
    public class CreateArticleViewModel
    {
        public string Headline { get; set; }
        public string Content { get; set; }
        public string ContentSummary { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateStamp { get; set; }
        public string SourceURL { get; set; }
        public bool IsArchived { get; set; }
        public bool CommentsOnOff { get; set; }



        public bool IsEditorsChoice { get; set; }



        // Tag fields
        [Required]
        public string TagName { get; set; } // Required tag name

        [Required]
        public string TagDescription { get; set; } // Required tag description
        [Required]
        public string CategoryName { get; set; } // For creating a new category
                                                 // Tag fields
        [Required]
        public string CategoryDescription { get; set; } // For creating a new categor
    }
}




