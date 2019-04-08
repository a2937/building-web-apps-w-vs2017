using SpyStore.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SpyStore.Models.Entities
{
    [Table("Orders", Schema = "Store")]
    public class Order : EntityBase
    {
        private decimal? _ordertotal;

        public int CustomerId { get; set; }

        [Display(Name = "Total")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? OrderTotal { get => OrderDetails.Sum(x => (x.Quantity * x.UnitCost)); private set { _ordertotal = value; } }

        [DataType(DataType.Date)]
        [Display(Name = "Date Ordered")]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Shipped")]
        public DateTime ShipDate { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        [InverseProperty(nameof(Order))]
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}