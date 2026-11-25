using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models.ViewModels;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Costing_Formula___KSPA.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CurrencyController(ApplicationDbContext db)
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

            IEnumerable<Currency> currencyList = _db.Currencies;
            return View(currencyList);
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
        public IActionResult Create(Currency currency)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (currency == null)
            {
                return NotFound();
            }

            // Check for unique field duplications.............................................................
            // Currency Code
            var codeDupCurr = _db.Currencies.FirstOrDefault(currencyEntry =>
                currencyEntry.CurrencyCode == currency.CurrencyCode
            );
            if (codeDupCurr != null)
            {
                ModelState.AddModelError(nameof(Currency.CurrencyCode), "Currency short code must be unique.");
            }
            // Currency Desc
            var descDupCurr = _db.Currencies.FirstOrDefault(currencyEntry =>
                currencyEntry.CurrencyDesc == currency.CurrencyDesc
            );
            if (descDupCurr != null)
            {
                ModelState.AddModelError(nameof(Currency.CurrencyDesc), "Currency description must be unique.");
            }

            if (ModelState.IsValid)
            {
                _db.Currencies.Add(currency);
                _db.SaveChanges();
                ViewBag.SuccessMessage = "Currency created successfully!";
            } else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(currency);
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
            var currency = _db.Currencies.Find(id);
            if (currency == null)
            {
                return NotFound();
            }
            return View(currency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Currency currency)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (currency == null || currency.CurrencyId == 0)
            {
                return NotFound();
            }

            //if (_db.Cylinders.Find(bagTypeVM.BagType.BagTypeId) == null)
            //{
            //    return NotFound();
            //}

            // Check for unique field duplications except for the selected.....................................
            // Currency Code
            var codeDupCurr = _db.Currencies.FirstOrDefault(currencyEntry =>
                (currencyEntry.CurrencyCode == currency.CurrencyCode)
                &&
                (currencyEntry.CurrencyId != currency.CurrencyId)
            );
            if (codeDupCurr != null)
            {
                ModelState.AddModelError(nameof(Currency.CurrencyCode), "There are other records with the same currency code.");
            }
            // Currency Desc
            var descDupCurr = _db.Currencies.FirstOrDefault(currencyEntry =>
                (currencyEntry.CurrencyDesc == currency.CurrencyDesc)
                &&
                (currencyEntry.CurrencyId != currency.CurrencyId)
            );
            if (descDupCurr != null)
            {
                ModelState.AddModelError(nameof(Currency.CurrencyDesc), "There are other records with the same currency description.");
            }

            if (ModelState.IsValid)
            {
                _db.Currencies.Update(currency);
                _db.SaveChanges();
                TempData["SuccessMessage"] = "Currency updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
                return View(currency);
            }
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<Currency> currencyList = new List<Currency>();
            currencyList.Add(new Currency()
            {
                CurrencyCode = "PlaceHolder",
                CurrencyDesc = "PlaceHolder",
                IsActive = false
            }); // This is a little hack
            return View(currencyList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<Currency> currencyList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (currencyList == null)
            {
                return View(currencyList);
            }

            if (ModelState.IsValid)
            {
                _db.Currencies.AddRange(currencyList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(currencyList);
        }
    }
}
