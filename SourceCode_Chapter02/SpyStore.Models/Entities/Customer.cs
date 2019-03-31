using SpyStore.Models.Entities.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyStore.Models.Entities
{
    [Table("Customers", Schema = "Store")]
    public class Customer : EntityBase
    {
        [DataType(DataType.Text), MaxLength(50), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Text), MaxLength(50), Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress), MaxLength(50), Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password), MaxLength(50)]
        public string Password { get; set; }

        [InverseProperty(nameof(Order.Customer))]
        public List<Order> Orders { get; set; } = new List<Order>();

        [InverseProperty(nameof(ShoppingCartRecord.Customer))]
        public virtual List<ShoppingCartRecord> ShoppingCartRecords { get; set; }
        = new List<ShoppingCartRecord>();
    }
}