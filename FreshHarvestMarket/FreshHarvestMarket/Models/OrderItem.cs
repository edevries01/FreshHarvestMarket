
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshHarvestMarket.Models
{
    //Edited OrderItem to complete class
    //Updated Model to remove redundant properties- TB 3/1/2026
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int ProduceId { get; set; } //changed from ProductId to ProduceId

        [Range(1, 100, ErrorMessage = "Please enter a quanity between 1 and 100.")]
        public int Quanity { get; set; }

        public decimal TotalPrice => Quanity * UnitPrice;

        //Foregin Key to Order
        public int OrderId { get; set; }

        //Nullable for cart use
        public Order? Order { get; set; }



    }
    
    
}
