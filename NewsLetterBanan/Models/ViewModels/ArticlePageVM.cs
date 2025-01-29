using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NewsLetterBanan.Data;

namespace NewsLetterBanan.Models.ViewModels
{

    public class ArticlePageVM
    {
        public ArticleContent ArticleContent { get; set; }
        public ArticleAuthorDetails ArticleAuthorDetails { get; set; }

        //we can add new properties if we want to include them to ArticlePageVM
    }
    public class ArticleContent
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateStamp { get; set; }
        public string ImageUrl { get; set; }
        public List<Tag> ArticleTags { get; set; } // List of Tags (name)
        public string Category { get; set; }
        public string SourceURL { get; set; }
        public bool CommentsOnOff { get; set; } // i dont know how should we make logic for switching comments on/off, but in case we have it in view model. We can use it inside View (page).
        public int Views { get; set; }
        public int Likes { get; set; }
    }
    public class ArticleAuthorDetails
    {
        public string AuthorsFirstAndLastName { get; set; }
    }


}