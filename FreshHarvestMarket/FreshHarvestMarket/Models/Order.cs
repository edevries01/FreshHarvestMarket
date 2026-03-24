using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreshHarvestMarket.Models
{
    /// <summary>
    /// Represents an order placed by a customer for FreshHarvest market
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Primary key for an order
        /// </summary>
        [Required]
        [Key]
        public int OrderId { get; set; } 

        /*
         * Do not include any user stuff yet
        public int? UserId {  get; set; }

        public User? User { get; set; }
        */

        /// <summary>
        /// Collection of order items associated with this order
        /// </summary>
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        /// <summary>
        /// The total of the order
        /// </summary>
        [Required]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// The date the order was placed
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        /// <summary>
        /// The date the order should be picked up from fresh harvest market
        /// </summary>
        [Required]
        public DateTime PickupDate { get; set; }

        /// <summary>
        /// True if the order has been picked up
        /// </summary>
        [Required]
        public bool IsPickedUp { get; set; }

        /// <summary>
        /// If true the order has been rejected by the operator/admin
        /// </summary>
        [Required]
        public bool Rejected { get; set; }

        /// <summary>
        /// Updates the order total to what it would be at time of execution
        /// Can be called right before confirming a customer's order to lock in the total 
        /// at the prices of the time
        /// </summary>
        public void UpdateOrderTotal()
        {
            //Order prices should not be updated once the order's life cycle is complete
            if (IsPickedUp || Rejected)
            {
                return;
            }

            decimal runningTotal = 0;
            foreach(OrderItem item in Items)
            {
                runningTotal += item.TotalPrice;
            }

            OrderTotal = runningTotal;
        }
    }
}
