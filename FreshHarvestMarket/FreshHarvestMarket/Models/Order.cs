using System;
using Stystem.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.Models
{
    public class Order
    {
        public int OrderId { get; set; } 

        public int? UserId {  get; set; } //Link to registered User. Null for non-memeber

        [Required]
        public int CustomerName { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        [Required]
        public decimal OrderTotal { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public bool IsPickedUp { get; set; } = false;


    }
}
