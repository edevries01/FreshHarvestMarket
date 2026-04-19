using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.Repositories;
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
        private IRepository<Discount> _DiscountRepository { get; set; }
        private IRepository<Produce> _ProduceRepository { get; set; }

        public DiscountController(IRepository<Discount> discountRepository, IRepository<Produce> produceRepository)
        {
            _DiscountRepository = discountRepository;
            _ProduceRepository = produceRepository;
        }

        /// <summary>
        /// Displays all existing/active discounts
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            List<Discount> discounts = _DiscountRepository.GetAll().Include(d => d.Produce).ToList();
            return View(discounts);
        }

        /// <summary>
        /// Returns the screen to add a new discount
        /// </summary>
        /// <returns>View for adding new discount</returns>
        public IActionResult Add()
        {
            List<int> existingDiscounts = _DiscountRepository.GetAll().Select(d => d.ProduceId).ToList();

            ViewBag.Action = "Add";

            //Only get products that don't alreday have a discount
            ViewBag.Produce = _ProduceRepository.GetAll()
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
            ViewBag.Produce = _ProduceRepository.GetAll().OrderBy(p => p.ProduceName).ToList();
            Discount? discount = _DiscountRepository.GetAll().Where(d => d.DiscountId == id).FirstOrDefault();

            if (discount == null)
                return RedirectToAction("Index");

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
            Discount? existingDiscount = _DiscountRepository.GetAll()
                .Where(d => d.ProduceId == discount.ProduceId)
                .FirstOrDefault();

            if (ModelState.IsValid)
            {
                //Delete old one in place of new one
                if (existingDiscount != null)
                {
                    _DiscountRepository.Delete(existingDiscount);
                }

                if (discount.DiscountId == 0)
                {
                    _DiscountRepository.Insert(discount);
                }
                else
                {
                    _DiscountRepository.Update(discount);
                }
                _DiscountRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Action = (discount.DiscountId == 0) ? "Add" : "Edit";
                ViewBag.Produce = _ProduceRepository.GetAll().OrderBy(p => p.ProduceName).ToList();
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
            Discount? discount = _DiscountRepository.GetAll().Where(d => d.DiscountId == id).FirstOrDefault();
            if (discount != null)
            {
                _DiscountRepository.Delete(discount);
                _DiscountRepository.Save();
            }
            return RedirectToAction("Index", "Discount");
        }


    }
}
