using NewsLetterBanan.Data;

namespace NewsLetterBanan.Models.ViewModels
{ 
 public class HomePageViewModel
{
    //   public WeatherData Weather { get; set; }
    public IEnumerable<Article> Latest { get; set; }
    public IEnumerable<Article> EditorsChoice { get; set; }
}

}