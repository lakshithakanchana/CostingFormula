using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class ItemCategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ItemCategoryController(ApplicationDbContext db)
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

            IEnumerable<ItemCategory> itemCatList = _db.ItemCategories;
            return View(itemCatList);
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemCategory iCategory)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (iCategory == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.ItemCategories.Add(iCategory);
                ViewBag.SuccessMessage = "Item Category created successfully!";
                _db.SaveChanges();
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(iCategory);
        }

        public IActionResult Update(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var iCategory = _db.ItemCategories.Find(id);
            if (iCategory == null)
            {
                return NotFound();
            }
            return View(iCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ItemCategory iCategory)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (iCategory == null || iCategory.ItemCatId == 0)
            {
                return NotFound();
            }
            //if (_db.ItemCategories.Find(iCategory.ItemCatId) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.ItemCategories.Update(iCategory);
                TempData["SuccessMessage"] = "Item Category updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(iCategory);
            }
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<ItemCategory> itemCatList = new List<ItemCategory>();
            itemCatList.Add(new ItemCategory()
            {
                ItemCategoryCode = "PH",
                ItemCategoryDesc = "PH",
                IsPolyComb = false,
                IsCorruComb = false,
                IsActive = false
            }); // This is a little hack
            return View(itemCatList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<ItemCategory> itemCatList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (itemCatList == null)
            {
                return View(itemCatList);
            }

            if (ModelState.IsValid)
            {
                _db.ItemCategories.AddRange(itemCatList);
                TempData["SuccessMessage"] = "Import successfull!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(itemCatList);
        }

    }
}
