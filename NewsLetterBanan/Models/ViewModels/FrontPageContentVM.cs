
namespace NewsLetterBanan.Models.ViewModels
{
    public class FrontPageArticlesVM
    {
        public List<ArticleItem> LatestArticles { get; set; }
        public List<ArticleItem> PopularArticles { get; set; }
        public List<ArticleItem> EditorsRecommendations { get; set; }

        // TODO: Front Page Content - We can add new properties here, that we want to show on Front Page.
    }
    public class ArticleItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DatePublished { get; set; }
        public string ContentSummary { get; set; }

        //TODO: Front Page Content  - Articles can be shown with Likes(amount) and Views(ammount), in the future.
    }

    public class CategoryItem
    {

    }
    public class TagItem
    {

    }

  

}

