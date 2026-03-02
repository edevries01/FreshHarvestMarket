
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

        public Produce? Produce { get; set; }

        [Range(1, 100, ErrorMessage = "Please enter a quantity between 1 and 100.")]
        public int Quantity { get; set; }

        public decimal TotalPrice => Produce != null ? Quantity * Produce.UnitPrice : 0;

        //Foregin Key to Order
        public int OrderId { get; set; }

        //Nullable for cart use
        public Order? Order { get; set; }



    }
    
    
}
