
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{
    public class SubscriptionsType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; } // FK

        [MaxLength(40)]
        public required string TypeName { get; set; }

        [MaxLength(1000)]
        public required string Description { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public required decimal Price { get; set; }
        public virtual ICollection<Subscription> Subscription { get; set; } = new List<Subscription>();
    }
}
