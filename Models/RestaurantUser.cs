using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mealify.Models
{
    public class RestaurantUser
    {
        public int RestaurantId { get; set; }
        public string ApplicationUserId { get; set; } = "";
        [ForeignKey("RestaurantId")]
        public Restaurant? Restaurant { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
