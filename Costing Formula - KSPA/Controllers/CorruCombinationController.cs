using Costing_Formula___KSPA.Models.ViewModels;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Costing_Formula___KSPA.Data;
using System.Text.Json;
using Costing_Formula___KSPA.Models.JsonModels;
using Microsoft.EntityFrameworkCore;

namespace Costing_Formula___KSPA.Controllers
{
    public class CorruCombinationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CorruCombinationController(ApplicationDbContext db)
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

            IEnumerable<CorruCombination> corruCombinationList = _db.CorruCombinations.Include(cc => cc.Color);

            return View(corruCombinationList);
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            // Getting View Model
            CorruCombinationVM CorruCombVM = new CorruCombinationVM()
            {
                CorruCombination = new CorruCombination()
                {
                    CorruCombItemCatBridges = new List<CorruCombItemCatBridge>
                    {
                        new()
                    }
                },
                ColorDropDown = _db.Colors.Where(c =>
                    c.IsActive
                ).Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PlacementDropDown = _db.Placements.Where(p => 
                    p.IsActive
                ).Select(item => new SelectListItem
                {
                    Text = item.PlacementCode,
                    Value = item.PlacementId.ToString()
                }),
                ItemCategoryDropDown = _db.ItemCategories.Where(record =>
                    record.IsActive
                    &&
                    record.IsCorruComb
                ).Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                })
            };
            return View(CorruCombVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CorruCombinationVM corruCombVM)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (corruCombVM == null || corruCombVM.CorruCombination == null /*|| corruCombVM.ItemCategor == null*/)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.CorruCombinations.Add(corruCombVM.CorruCombination);
                ViewBag.SuccessMessage = "Corru Combination created successfully!";
                _db.SaveChanges();

                return RedirectToAction("View", "CorruCombination", new { id = corruCombVM.CorruCombination.CorruCombId });
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            // To send View(completeObject). Otherwise selectLists will not fill
            corruCombVM.ItemCategoryDropDown = _db.ItemCategories.Where(record =>
                    record.IsActive == true
                    &&
                    record.IsCorruComb == true
                ).Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                });
            corruCombVM.ColorDropDown = _db.Colors.Select(item => new SelectListItem
            {
                Text = item.ColorName,
                Value = item.ColorId.ToString()
            });
            corruCombVM.PlacementDropDown = _db.Placements.Where(p =>
                p.IsActive
            ).Select(item => new SelectListItem
            {
                Text = item.PlacementCode,
                Value = item.PlacementId.ToString()
            });

            return View(corruCombVM);
        }

        public IActionResult View(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var corruComb = _db.CorruCombinations.Find(id);
            if (corruComb == null)
            {
                return NotFound();
            }

            var corruCombVM = new CorruCombinationVM()
            {
                CorruCombination = corruComb,
                ColorDropDown = _db.Colors.Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PlacementDropDown = _db.Placements.Select(item => new SelectListItem
                {
                    Text = item.PlacementCode,
                    Value = item.PlacementId.ToString()
                }),
                ItemCategoryDropDown = _db.ItemCategories.Where(record =>
                    record.IsCorruComb
                ).Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                })
            };

            corruCombVM.CorruCombination.CorruCombItemCatBridges = _db.CorruCombItemCatBridges.Where(record =>
                record.CorruCombId == id
            ).ToList();

            List<CorruCombItemCatBridgeJsonM> pcicBridgeJList = new List<CorruCombItemCatBridgeJsonM>();
            foreach (var pcicBridge in corruCombVM.CorruCombination.CorruCombItemCatBridges)
            {
                pcicBridgeJList.Add(new CorruCombItemCatBridgeJsonM()
                {
                    PlacementId = pcicBridge.PlacementId,
                    Gsm = pcicBridge.Gsm,
                    ItemCatId = pcicBridge.ItemCatId
                });
            }

            corruCombVM.CorruCombItemCatBridgesJSONArray = JsonSerializer.Serialize(pcicBridgeJList);
            Console.WriteLine(corruCombVM.CorruCombItemCatBridgesJSONArray);

            return View(corruCombVM);
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<CorruCombItemCatBridge> corruCombItemCatBridgeList = new List<CorruCombItemCatBridge>();
            corruCombItemCatBridgeList.Add(new CorruCombItemCatBridge()
            {
                CorruCombId = 0,
                PlacementId = 0,
                ItemCatId = 51,
                Gsm = 10
            });

            List<CorruCombinationVM> corruCombinationVMList = new List<CorruCombinationVM>();
            corruCombinationVMList.Add(new CorruCombinationVM()
            {
                CorruCombination = new CorruCombination()
                {
                    CorruCombCode = "PlH",
                    ColorId = 0,
                    NoOfPlys = 0,
                    IsActive = false,
                    CorruCombItemCatBridges = corruCombItemCatBridgeList
                },
                ColorDropDown = _db.Colors.Where(c =>
                    c.IsActive
                ).Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PlacementDropDown = _db.Placements.Where(p =>
                    p.IsActive
                ).Select(item => new SelectListItem
                {
                    Text = item.PlacementCode,
                    Value = item.PlacementId.ToString()
                }),
                ItemCategoryDropDown = _db.ItemCategories.Where(record =>
                    record.IsActive
                    &&
                    record.IsCorruComb
                ).Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                })
            }); // This is a little hack

            var colorList = _db.Colors.Where(c =>
                    c.IsActive
                ).Select(item => new IdTextJsonM
                {
                    Id = item.ColorId,
                    Text = item.ColorName
                }).ToList<IdTextJsonM>();
            ViewBag.colorsJsonStr = JsonSerializer.Serialize(colorList);

            var placementList = _db.Placements.Where(p =>
                    p.IsActive
                ).Select(item => new IdTextJsonM
                {
                    Id = item.PlacementId,
                    Text = item.PlacementCode
                }).ToList<IdTextJsonM>();
            ViewBag.placementsJsonStr = JsonSerializer.Serialize(placementList);

            var itemCategoryList = _db.ItemCategories.Where(record =>
                    record.IsActive
                    &&
                    record.IsCorruComb
                ).Select(item => new IdTextJsonM
                {
                    Id = item.ItemCatId,
                    Text = item.ItemCategoryDesc
                }).ToList<IdTextJsonM>();
            ViewBag.itemCategoriesJsonStr = JsonSerializer.Serialize(itemCategoryList);

            return View(corruCombinationVMList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<CorruCombinationVM> corruCombinationVMList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (corruCombinationVMList == null)
            {
                return View(corruCombinationVMList);
            }

            List<CorruCombination> corruCombinationList = new List<CorruCombination>();
            foreach (var corruCombinationVM in corruCombinationVMList)
            {
                if (corruCombinationVM != null)
                {
                    if (corruCombinationVM.CorruCombination != null)
                        corruCombinationList.Add(corruCombinationVM.CorruCombination);
                }
            }

            if (corruCombinationList.Count == 0)
            {
                return View(corruCombinationVMList);
            }

            if (corruCombinationList == null)
            {
                return View(corruCombinationVMList);
            }

            var groupedList = corruCombinationList.GroupBy(properties => properties.CorruCombCode).ToList();
            List<CorruCombination> groupedCorruCombinationList = new List<CorruCombination>();
            foreach (var groupedItem in groupedList)
            {
                CorruCombination corruCombination = groupedItem.First();
                List<CorruCombItemCatBridge> corruCombItemCatBridgeList = new List<CorruCombItemCatBridge>();
                foreach (var item in groupedItem)
                {
                    if (item != null)
                    {
                        if (item.CorruCombItemCatBridges != null)
                        {
                            corruCombItemCatBridgeList.Add(
                                item.CorruCombItemCatBridges[0]
                            );
                        }
                    }
                }
                corruCombination.CorruCombItemCatBridges = corruCombItemCatBridgeList;
                groupedCorruCombinationList.Add(corruCombination);
            }

            if (ModelState.IsValid)
            {
                _db.CorruCombinations.AddRange(groupedCorruCombinationList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

                var colorList = _db.Colors.Where(c =>
                    c.IsActive
                ).Select(item => new IdTextJsonM
                {
                    Id = item.ColorId,
                    Text = item.ColorName
                }).ToList<IdTextJsonM>();
                ViewBag.colorsJsonStr = JsonSerializer.Serialize(colorList);

                var placementList = _db.Placements.Where(p =>
                        p.IsActive
                    ).Select(item => new IdTextJsonM
                    {
                        Id = item.PlacementId,
                        Text = item.PlacementCode
                    }).ToList<IdTextJsonM>();
                ViewBag.placementsJsonStr = JsonSerializer.Serialize(placementList);

                var itemCategoryList = _db.ItemCategories.Where(record =>
                        record.IsActive
                        &&
                        record.IsCorruComb
                    ).Select(item => new IdTextJsonM
                    {
                        Id = item.ItemCatId,
                        Text = item.ItemCategoryDesc
                    }).ToList<IdTextJsonM>();
                ViewBag.itemCategoriesJsonStr = JsonSerializer.Serialize(itemCategoryList);

                return View(corruCombinationVMList);
            }
        }
    }

}
