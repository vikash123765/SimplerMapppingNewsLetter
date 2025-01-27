using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsLetterBanan.Data
{
    public class Subscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        public required int SubscriptionsTypeId { get; set; } // FK
        public virtual SubscriptionsType SubscriptionsType { get; set; } // Navigation property (relation TO ONE)

        [Column(TypeName = "decimal(5, 2)")]
        public required decimal Price { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public required DateTime Expires { get; set; }

        [Required]
        public required string UserId { get; set; } // FK to User 
        public virtual User User { get; set; } // Navigation property (relation TO ONE)

        public required bool PaymentComplete { get; set; } = false;
        public required bool RenewalReminderSent { get; set; } = false;
    }
}
