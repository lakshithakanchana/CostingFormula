using Costing_Formula___KSPA.Models.ViewModels;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;
using Costing_Formula___KSPA.Data;

namespace Costing_Formula___KSPA.Controllers
{
    public class ExchangeRateController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExchangeRateController(ApplicationDbContext db)
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
            var activeCurrencyList = _db.Currencies
                .Where(record =>
                    record.IsActive
                );

            List<ExchangeRate> finalExchangeRateList = new();

            foreach (var activeCurrency in activeCurrencyList)
            {
                ExchangeRate? exR = _db.ExchangeRates
                    .Where(recordE =>
                        recordE.CurrencyId == activeCurrency.CurrencyId
                        &&
                        recordE.IsActive
                        &&
                        recordE.EffectiveFrom <= DateTime.Now
                    ).OrderByDescending(fields =>
                        fields.EffectiveFrom
                    ).FirstOrDefault();

                // If exR is null
                exR ??= new ExchangeRate
                    {
                        Currency = activeCurrency,
                        CurrencyId = activeCurrency.CurrencyId,
                        ExchangeRateId = 0, // Later I can check for this property in the view to indicate not ex-rate for the currency
                        RateInLKR = 0 // Later I can check for this property in the view to indicate not ex-rate for the currency
                    };

                finalExchangeRateList.Add(exR);
            }

            return View(finalExchangeRateList);
        }

        public IActionResult Create(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            // Here currencyId is which we suppose to add an ExchangeRate (daily / monthly)
            if (id == null || id == 0)
                return NotFound();

            Currency? currency = _db.Currencies.Find(id);
            if (currency == null)
                return NotFound();

            // Getting View Model
            ExchangeRateVM exchangeRateVM = new ExchangeRateVM()
            {
                ExchangeRate = new ExchangeRate
                {
                    CurrencyId = (int)id,
                    Currency = currency,
                    EffectiveFrom = DateTime.Now,
                    RateInLKR = null
                },
                ExchangeRateList = _db.ExchangeRates.Where(exchangeRateEntry =>
                    //exchangeRateEntry.IsActive // All active and inactive rates will be listed, so that I can put href to edit them
                    //&&
                    exchangeRateEntry.CurrencyId == id
                )
            };

            return View(exchangeRateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExchangeRateVM exchangeRateVM)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (
                exchangeRateVM == null 
                || exchangeRateVM.ExchangeRate == null 
                || exchangeRateVM.ExchangeRate.CurrencyId == null // Do not remove because it is nullable when submitting empty form
                || exchangeRateVM.ExchangeRate.CurrencyId == 0
                )
                return NotFound();

            exchangeRateVM.ExchangeRate.EffectiveFrom = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.ExchangeRates.Add(exchangeRateVM.ExchangeRate);
                ViewBag.SuccessMessage = "Exchange rate added successfully!";
                _db.SaveChanges();
            } else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            // To send View(completeObject). Otherwise selectLists will not fill
            Currency? currency = _db.Currencies.Find(exchangeRateVM.ExchangeRate.CurrencyId);
            if (currency == null)
                return NotFound();
            exchangeRateVM.ExchangeRateList = _db.ExchangeRates.Where(exchangeRateEntry =>
                exchangeRateEntry.IsActive
            );
            IEnumerable<ExchangeRate> exchangeRateList = _db.ExchangeRates.Where(exchangeRateEntry =>
                    exchangeRateEntry.IsActive
                    &&
                    exchangeRateEntry.CurrencyId == exchangeRateVM.ExchangeRate.CurrencyId
            );

            exchangeRateVM.ExchangeRate.Currency = currency;
            exchangeRateVM.ExchangeRateList = exchangeRateList;

            return View(exchangeRateVM);
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
            var exchangeRate = _db.ExchangeRates.Find(id);
            if (exchangeRate == null)
            {
                return NotFound();
            }
            exchangeRate.Currency = _db.Currencies.Find(exchangeRate.CurrencyId);
            if (exchangeRate.Currency == null)
            {
                return NotFound();
            }
            return View(exchangeRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ExchangeRate exchangeRate)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (exchangeRate == null || exchangeRate.ExchangeRateId == 0 || exchangeRate.CurrencyId == 0)
            {
                return NotFound();
            }

            exchangeRate.EffectiveFrom = DateTime.Now;

            if (ModelState.IsValid)
            {
                _db.ExchangeRates.Update(exchangeRate);
                TempData["SuccessMessage"] = "Exchange rate updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Create", new { id = exchangeRate.CurrencyId }); //Success
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
                exchangeRate.Currency = _db.Currencies.Find(exchangeRate.CurrencyId);
                return View(exchangeRate);
            }
        }
    }
}
