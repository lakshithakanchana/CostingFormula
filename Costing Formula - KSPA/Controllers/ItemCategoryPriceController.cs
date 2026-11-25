using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class ItemCategoryPriceController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ItemCategoryPriceController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            // Getting all the active currencies
            var activeItemCategoryList = _db.ItemCategories
                .Where(record =>
                    record.IsActive
                );

            List<ItemCategoryPrice> finalItemCategoryPriceList = new();

            foreach (var activeItemCategory in activeItemCategoryList)
            {
                ItemCategoryPrice? iCatP = _db.ItemCategoryPrices
                    .Where(recordE =>
                        recordE.ItemCatId == activeItemCategory.ItemCatId
                        &&
                        recordE.IsActive
                        &&
                        recordE.DateUpdated <= DateTime.Now
                    ).OrderByDescending(fields =>
                        fields.DateUpdated
                    ).FirstOrDefault();

                // If iCatP is null
                iCatP ??= new ItemCategoryPrice
                {
                    ItemCategory = activeItemCategory,
                    ItemCatId = activeItemCategory.ItemCatId,
                    IcpId = 0, // Later I can check for this property in the view to indicate not ex-rate for the currency
                    UnitPrice = 0, // Later I can check for this property in the view to indicate not ex-rate for the currency
                    UnitDuty = 0
                };

                finalItemCategoryPriceList.Add(iCatP);
            }

            return View(finalItemCategoryPriceList);
        }

        public IActionResult Create(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            // Here Id is the ItemCategoryId
            if (id == null || id == 0)
                return NotFound();

            ItemCategory? itemCategory = _db.ItemCategories.Find(id);
            if (itemCategory == null)
                return NotFound();

            ItemCategoryPrice itemCategoryPrice = new ItemCategoryPrice
            {
                ItemCatId = itemCategory.ItemCatId,
                DateUpdated = DateTime.Now,
                ItemCategory = itemCategory
            };
            
            return View(itemCategoryPrice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemCategoryPrice itemCategoryPrice)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (
                itemCategoryPrice == null
                || itemCategoryPrice.ItemCatId == null // Do not remove because it is nullable when submitting empty form
                || itemCategoryPrice.ItemCatId == 0
                )
                return NotFound();

            itemCategoryPrice.DateUpdated = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.ItemCategoryPrices.Add(itemCategoryPrice);
                ViewBag.SuccessMessage = "Price added successfully!";
                _db.SaveChanges();
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            // To send View(completeObject). Otherwise selectLists will not fill
            itemCategoryPrice.ItemCategory = _db.ItemCategories.Find(itemCategoryPrice.ItemCatId);
            if (itemCategoryPrice.ItemCategory == null)
                return NotFound();

            return View(itemCategoryPrice);
        }
    }
}
