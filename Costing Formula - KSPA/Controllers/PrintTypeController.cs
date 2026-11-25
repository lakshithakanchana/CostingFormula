using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class PrintTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PrintTypeController(ApplicationDbContext db)
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

            IEnumerable<PrintType> printTypeList = _db.PrintTypes;
            return View(printTypeList);
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
        public IActionResult Create(PrintType printType)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (printType == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.PrintTypes.Add(printType);
                ViewBag.SuccessMessage = "Print Type added successfully!";
                _db.SaveChanges();
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(printType);
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
            var printType = _db.PrintTypes.Find(id);
            if (printType == null)
            {
                return NotFound();
            }
            return View(printType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(PrintType printType)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (printType == null || printType.PrintTypeId == 0)
            {
                return NotFound();
            }
            //if (_db.Colors.Find(color.ColorId) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.PrintTypes.Update(printType);
                TempData["SuccessMessage"] = "Print Type updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(printType);
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<PrintType> printTypeList = new List<PrintType>();
            printTypeList.Add(new PrintType()
            {
                PrintTypeCode = "PlH",
                ConsumptionLitrePerSqrCm = 0.0,
                PrintPercentage = 0.0,
                WithoutCylinderBase = false,
                IsActive = false,
            }); // This is a little hack
            return View(printTypeList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<PrintType> printTypeList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (printTypeList == null)
            {
                return View(printTypeList);
            }

            if (ModelState.IsValid)
            {
                _db.PrintTypes.AddRange(printTypeList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(printTypeList);
        }
    }
}
