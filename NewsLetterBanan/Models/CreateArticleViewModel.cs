using System;
using System.Collections.Generic;
using NewsLetterBanan.Data;
using System.ComponentModel.DataAnnotations;

namespace NewsLetterBanan.Models
{
    public class CreateArticleViewModel
    {
        public string Headline { get; set; }
        public string Content { get; set; }
        public string ContentSummary { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateStamp { get; set; }
        public string Category { get; set; } // Category is a simple string
        public string SourceURL { get; set; }
        public bool IsArchived { get; set; }
        public bool CommentsOnOff { get; set; }

        public bool IsEditorsChoice { get; set; }

        [Required]
        public string TagName { get; set; } // Required tag name

        [Required]
        public string TagDescription { get; set; } // Required tag description
    }
}




