using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FreshHarvestMarket.Controllers
{
    /// <summary>
    /// Handles requests related to discounts
    /// This whole controller should only be accessible by the operator/admin
    /// </summary>
    public class DiscountController : Controller
    {
        /// <summary>
        /// Context service for movie entities
        /// </summary>
        private FreshMarketContext context { get; set; }

        public DiscountController(FreshMarketContext ctx)
        {
            context = ctx;
        }

        /// <summary>
        /// Displays all existing/active discounts
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<Discount> discounts = context.Discounts.Include(d => d.Produce).ToList();
            return View(discounts);
        }

        /// <summary>
        /// Returns the screen to add a new discount
        /// </summary>
        /// <returns>View for adding new discount</returns>
        public IActionResult Add()
        {
            List<int> existingDiscounts = context.Discounts.Select(d => d.ProduceId).ToList();

            ViewBag.Action = "Add";

            //Only get products that don't alreday have a discount
            ViewBag.Produce = context.Produce
                .Where(p => !existingDiscounts.Contains(p.ProduceId))
                .OrderBy(p => p.ProduceName).ToList();

            return View("Edit", new Discount());
        }

        /// <summary>
        /// Returns the view for editing a discount
        /// </summary>
        /// <param name="id">ID of discount to edit</param>
        /// <returns>View  for editing a discount</returns>
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Produce = context.Produce.OrderBy(p => p.ProduceName).ToList();
            Discount discount = context.Discounts.Find(id);
            return View(discount);
        }

        /// <summary>
        /// Validates and edits submitted discount
        /// </summary>
        /// <param name="discount">The discount to edit</param>
        /// <returns>Edit screen if invalid and discount index screen if valid</returns>
        [HttpPost]
        public IActionResult Edit(Discount discount)
        {
            //Check if this item already has a discount
            Discount existingDiscount = context.Discounts
                .Where(d => d.ProduceId == discount.ProduceId)
                .FirstOrDefault();

            if (existingDiscount != null)
            {
                ModelState.AddModelError(nameof(discount.ProduceId), "This produce already has a discount");
            }

            if (ModelState.IsValid)
            {
                

                if (discount.DiscountId == 0)
                {
                    context.Discounts.Add(discount);
                }
                else
                {
                    context.Discounts.Update(discount);
                }
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Action = (discount.DiscountId == 0) ? "Add" : "Edit";
                ViewBag.Produce = context.Produce.OrderBy(p => p.ProduceName).ToList();
                return View(discount);
            }
        }

        /// <summary>
        /// Deletes a discount
        /// </summary>
        /// <param name="discount">Discount to delete</param>
        /// <returns>Discounts index view</returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Discount discount = context.Discounts.Find(id);
            if (discount != null)
            {
                context.Discounts.Remove(discount);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


    }
}
