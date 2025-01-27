using NewsLetterBanan.Data;

namespace NewsLetterBanan.Services.Interfaces
{
    public interface IArticleService
    {
        Task<List<Article>> GetLatestArticlesAsync();
        Task AddArticleAsync(Article article);
        Task<Article> GetArticleByIdAsync(int id);
        Task DeleteArticleAsync(int id);
    }
}
