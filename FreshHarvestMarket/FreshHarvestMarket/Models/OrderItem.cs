
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshHarvestMarket.Models
{
    //Edited OrderItem to complete class
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        [Range(1, 100, ErrorMessage = "Please enter a quanity between 1 and 100.")]
        public int Quanity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quanity * UnitPrice;

        //Foregin Key to Order
        public int OrderId { get; set; }

        //Nullable for cart use
        public Order? Order { get; set; }



    }
    
    
}
