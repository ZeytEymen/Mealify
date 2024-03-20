using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mealify.Models
{
    public class Company
    {
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(200)")]
        public string Name { get; set; } = "";

        [Column(TypeName = "char(5)")]
        [StringLength(5, MinimumLength = 5)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; } = "";

        [StringLength(200, MinimumLength = 5)]
        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; } = "";

        [Phone]
        [Column(TypeName = "varchar(30)")]
        public string Phone { get; set; } = "";

        [StringLength(100)]
        [EmailAddress]
        [Column(TypeName = "varchar(100)")]
        public string EMail { get; set; } = "";

        [StringLength(100, MinimumLength = 8)]
        [Column(TypeName = "varchar(100)")]
        public string? WebAddress { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "smalldatetime")]
        public DateTime RegisterDate { get; set; }
        
        [StringLength(11, MinimumLength = 10)]
        [Column(TypeName = "varchar(11)")]
        public string TaxNumber { get; set; } = "";

        public byte StateId { get; set; }

        [ForeignKey("StateId")]
        public State? State { get; set; }

        public int? ParentCompanyId { get; set; }

        [ForeignKey("ParentCompanyId")]
        public Company? ParentCompany { get; set; }

        public List<Restaurant>? Restaurants { get; set; }


    }
}
