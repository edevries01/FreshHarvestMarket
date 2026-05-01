/*
 * OrderItem.cs
 * FreshHarvestMarket
 *
 * This class represents a single item within an order.
 *
 * It links a purchased produce item to an order & stores:
 * - The selected produce item (via ProduceId & navigation property)
 * - Quantity purchased
 * - Calculated total price for the line item
 * - Relationship back to the parent Order
 *
 * This model supports both order history & shopping cart functionality,
 * where Order may be null until the item is finalized into an order.
 */

using System.ComponentModel.DataAnnotations;
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

        //Foreign Key to Order
        public int OrderId { get; set; }

        //Nullable for cart use
        public Order? Order { get; set; }
    }
    
    
}
