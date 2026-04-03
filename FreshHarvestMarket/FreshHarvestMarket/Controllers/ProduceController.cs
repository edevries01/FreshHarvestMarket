using FreshHarvestMarket.Data;
using FreshHarvestMarket.Models;
using FreshHarvestMarket.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreshHarvestMarket.Controllers
{
    public class ProduceController : Controller
    {
        private FreshMarketContext _context;

        public ProduceController(FreshMarketContext context)
        {
            _context = context;
        }

        //Retrieves a list of produce from the database, filtered by catgetory (optional)
        public IActionResult Index(string category = "All")
        {
            var produceQuery = _context.Produce.AsQueryable();

            if (category != "All")
            {
                produceQuery = produceQuery.Where(p => p.ProduceCategory == category);
            }

            var viewModel = new BrowseProduceViewModel
            {
                ProduceItems = produceQuery.ToList(),
                SelectedCategory = category,
                Categories = _context.Produce
                    .Select(p => p.ProduceCategory)
                    .Distinct()
                    .ToList()
            };
            return View(viewModel);

        }

        //Finds produce item by ID. Returns NotFound if item not found.
        public IActionResult Details(int id)
        {
            var item = _context.Produce.Find(id);
            if (item == null) return NotFound();
            return View(item);
        }
    }
}
