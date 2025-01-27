
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{
    public class WeatherForecast
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }
        public required int Summary { get; set; }
        public required int TemperatureC { get; set; }
        public required int Humidity { get; set; }
        public required int WindSpeed { get; set; }
        public required DateOnly Date { get; set; }
        public required string City { get; set; }
        public required string Description { get; set; }
        public virtual User User { get; set; }
    }
}
