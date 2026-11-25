using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;

namespace Costing_Formula___KSPA.Controllers
{
    public class SRepOrCoordinatorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SRepOrCoordinatorController(ApplicationDbContext db)
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

            IEnumerable<SRepOrCoordinator> sROP = _db.SRepsOrCoordinators;
            return View(sROP);
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
        public IActionResult Create(SRepOrCoordinator sROC)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (sROC == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.SRepsOrCoordinators.Add(sROC);
                ViewBag.SuccessMessage = "Record added successfully!";
                _db.SaveChanges();
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(sROC);
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
            var sROC = _db.SRepsOrCoordinators.Find(id);
            if (sROC == null)
            {
                return NotFound();
            }
            return View(sROC);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(SRepOrCoordinator sROC)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (sROC == null || sROC.SrocId == 0)
            {
                return NotFound();
            }
            //if (_db.SRepsOrCoordinators.Find(sROC.SrocId) == null)
            //{
            //    return NotFound();
            //}
            if (ModelState.IsValid)
            {
                _db.SRepsOrCoordinators.Update(sROC);
                TempData["SuccessMessage"] = "Record updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";
                return View(sROC);
            }
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<SRepOrCoordinator> sRepOrCoordinatorList = new List<SRepOrCoordinator>();
            sRepOrCoordinatorList.Add(new SRepOrCoordinator()
            {
                SrocName = "PH",
                SrocContact = "123",
                SrocEmail = "ph@ph.ph",
                IsReprecentative = false,
                IsCoordinator = false,
                IsActive = false
            }); // This is a little hack
            return View(sRepOrCoordinatorList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<SRepOrCoordinator> sRepOrCoordinatorList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (sRepOrCoordinatorList == null)
            {
                return View(sRepOrCoordinatorList);
            }

            if (ModelState.IsValid)
            {
                _db.SRepsOrCoordinators.AddRange(sRepOrCoordinatorList);
                TempData["SuccessMessage"] = "Import successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(sRepOrCoordinatorList);
        }
    }
}
