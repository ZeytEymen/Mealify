using Mealify.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mealify.Models
{
    public class FoodMenus
    {
        public int MenuId { get; set; }
        public int ContentId { get; set; }

        [ForeignKey("MenuId")]
        public Food? FoodMenu { get; set; }

        [ForeignKey("ContentId")]
        public Food? FoodContent { get; set; }
    }
}
