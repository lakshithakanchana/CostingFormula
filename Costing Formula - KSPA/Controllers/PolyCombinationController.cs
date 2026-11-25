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
    public class PolyCombinationController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PolyCombinationController(ApplicationDbContext db)
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

            IEnumerable<PolyCombination> polyCombinationList = _db.PolyCombinations
                .Include(pc => pc.Color)
                .Include(pc => pc.PolyMaterial);

            return View(polyCombinationList);
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            // Getting View Model
            PolyCombinationVM PolyCombVM = new PolyCombinationVM()
            {
                PolyCombination = new PolyCombination(),
                ItemCategoryDropDown = _db.ItemCategories.Where(record =>
                    record.IsActive == true
                    &&
                    record.IsPolyComb == true
                ).Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                }),
                ColorDropDown = _db.Colors.Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PolyMaterialDropDown =  _db.PolyMaterials.Select(item => new SelectListItem
                {
                    Text = item.PolyMatCode,
                    Value = item.PolyMaterialId.ToString()
                })
            };
            return View(PolyCombVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PolyCombinationVM polyCombVM)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (polyCombVM == null || polyCombVM.PolyCombination == null /*|| polyCombVM.ItemCategor == null*/)
            {
                return NotFound();
            }

            double sumOfPercentage = 0;
            foreach (var pcicBridge in polyCombVM.PolyCombination.PolyCombItemCatBridges)
            {
                sumOfPercentage += pcicBridge.Percentage;
            }
            if (sumOfPercentage != 100.00d)
                ModelState.AddModelError("", "Sum of percentages is " + sumOfPercentage + "%, not equal to 100.00%.");

            if (ModelState.IsValid)
            {
                _db.PolyCombinations.Add(polyCombVM.PolyCombination);
                ViewBag.SuccessMessage = "Poly Combination created successfully!";
                _db.SaveChanges();

                return RedirectToAction("View", "PolyCombination", new { id = polyCombVM.PolyCombination.PolyCombId });
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            // To send View(completeObject). Otherwise selectLists will not fill
            polyCombVM.ItemCategoryDropDown = _db.ItemCategories.Where(record =>
                    record.IsActive == true
                    &&
                    record.IsPolyComb == true
                ).Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                });
            polyCombVM.ColorDropDown = _db.Colors.Select(item => new SelectListItem
            {
                Text = item.ColorName,
                Value = item.ColorId.ToString()
            });
            polyCombVM.PolyMaterialDropDown = _db.PolyMaterials.Select(item => new SelectListItem
            {
                Text = item.PolyMatCode,
                Value = item.PolyMaterialId.ToString()
            });

            return View(polyCombVM);
        }

        public IActionResult View(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var polyComb = _db.PolyCombinations.Find(id);
            if (polyComb == null)
            {
                return NotFound();
            }

            var polyCombVM = new PolyCombinationVM()
            {
                PolyCombination = polyComb,
                ItemCategoryDropDown = _db.ItemCategories.Where(record =>
                    record.IsActive == true
                    &&
                    record.IsPolyComb == true
                ).Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                }),
                ColorDropDown = _db.Colors.Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                PolyMaterialDropDown = _db.PolyMaterials.Select(item => new SelectListItem
                {
                    Text = item.PolyMatCode,
                    Value = item.PolyMaterialId.ToString()
                })
            };

            polyCombVM.PolyCombination.PolyCombItemCatBridges = _db.PolyCombItemCatBridges.Where(record =>
                record.PolyCombId == id
            ).ToList();

            List<PolyCombItemCatBridgeJsonM> pcicBridgeJList = new List<PolyCombItemCatBridgeJsonM>();
            foreach (var pcicBridge in polyCombVM.PolyCombination.PolyCombItemCatBridges)
            {
                pcicBridgeJList.Add(new PolyCombItemCatBridgeJsonM()
                {
                    ItemCatId = pcicBridge.ItemCatId,
                    Percentage = pcicBridge.Percentage
                });
            }
            
            polyCombVM.PolyCombItemCatBridgesJSONArray = JsonSerializer.Serialize(pcicBridgeJList);
            Console.WriteLine(polyCombVM.PolyCombItemCatBridgesJSONArray);

            return View(polyCombVM);
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<PolyCombItemCatBridge> polyCombItemCatBridgeList = new List<PolyCombItemCatBridge>();
            polyCombItemCatBridgeList.Add(new PolyCombItemCatBridge()
            {
                PolyCombId = 0,
                ItemCatId = 51,
                Percentage = 10
            });

            List<PolyCombinationVM> polyCombinationVMList = new List<PolyCombinationVM>();
            polyCombinationVMList.Add(new PolyCombinationVM()
            {
                PolyCombination = new PolyCombination()
                {
                    PolyCombCode = "PlH",
                    ColorId = 0,
                    PolyMaterialId = 0,
                    PolyCombDesc = "PlH",
                    SpecialRef = "PlH",
                    IsActive = false,
                    PolyCombItemCatBridges = polyCombItemCatBridgeList
                },
                ColorDropDown = _db.Colors.Select(item => new SelectListItem
                {
                    Text = item.ColorName,
                    Value = item.ColorId.ToString()
                }),
                ItemCategoryDropDown = _db.ItemCategories.Select(item => new SelectListItem
                {
                    Text = item.ItemCategoryDesc,
                    Value = item.ItemCatId.ToString()
                })
            }); // This is a little hack

            var colorList = _db.Colors
                .Select(item => new IdTextJsonM
                {
                    Id = item.ColorId,
                    Text = item.ColorName
                }).ToList<IdTextJsonM>();
            ViewBag.colorsJsonStr = JsonSerializer.Serialize(colorList);

            var polyMatList = _db.PolyMaterials
                .Select(item => new IdTextJsonM
                {
                    Id = item.PolyMaterialId,
                    Text = item.PolyMatCode
                }).ToList<IdTextJsonM>();
            ViewBag.polyMaterialsJsonStr = JsonSerializer.Serialize(polyMatList);

            var itemCategoryList = _db.ItemCategories
                .Select(item => new IdTextJsonM
                {
                    Id = item.ItemCatId,
                    Text = item.ItemCategoryDesc
                }).ToList<IdTextJsonM>();
            ViewBag.itemCategoriesJsonStr = JsonSerializer.Serialize(itemCategoryList);

            return View(polyCombinationVMList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<PolyCombinationVM> polyCombinationVMList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (polyCombinationVMList == null)
            {
                return View(polyCombinationVMList);
            }

            List<PolyCombination> polyCombinationList = new List<PolyCombination>();
            foreach (var polyCombinationVM in polyCombinationVMList)
            {
                if (polyCombinationVM != null)
                {
                    if (polyCombinationVM.PolyCombination != null)
                        polyCombinationList.Add(polyCombinationVM.PolyCombination);
                }
            }

            if (polyCombinationList.Count == 0)
            {
                return View(polyCombinationVMList);
            }

            if (polyCombinationList == null)
            {
                return View(polyCombinationVMList);
            }

            var groupedList = polyCombinationList.GroupBy(properties => properties.PolyCombCode).ToList();
            List<PolyCombination> groupedPolyCombinationList = new List<PolyCombination>();
            foreach (var groupedItem in groupedList)
            {
                PolyCombination polyCombination = groupedItem.First();
                List<PolyCombItemCatBridge> polyCombItemCatBridgeList = new List<PolyCombItemCatBridge>();
                double sumOfPercentage = 0.000000d;
                foreach (var item in groupedItem)
                {
                    if(item != null)
                    {
                        if (item.PolyCombItemCatBridges != null)
                        {
                            polyCombItemCatBridgeList.Add(
                                item.PolyCombItemCatBridges[0]
                            );
                            sumOfPercentage += item.PolyCombItemCatBridges[0].Percentage;
                        }
                    }
                }
                if (sumOfPercentage > 100.0000001d || sumOfPercentage < 99.9999999)
                {
                    ModelState.AddModelError("", "Poly comb. code: " + polyCombination.PolyCombCode + ", Sum of percentages is " + sumOfPercentage + "%, not equal to 100.00%.");
                }
                polyCombination.PolyCombItemCatBridges = polyCombItemCatBridgeList;
                groupedPolyCombinationList.Add(polyCombination);
            }

            if (ModelState.IsValid)
            {
                _db.PolyCombinations.AddRange(groupedPolyCombinationList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

                var colorList = _db.Colors
                .Select(item => new IdTextJsonM
                {
                    Id = item.ColorId,
                    Text = item.ColorName
                }).ToList<IdTextJsonM>();
                ViewBag.colorsJsonStr = JsonSerializer.Serialize(colorList);

                var polyMatList = _db.PolyMaterials
                    .Select(item => new IdTextJsonM
                    {
                        Id = item.PolyMaterialId,
                        Text = item.PolyMatCode
                    }).ToList<IdTextJsonM>();
                ViewBag.polyMaterialsJsonStr = JsonSerializer.Serialize(polyMatList);

                var itemCategoryList = _db.ItemCategories
                    .Select(item => new IdTextJsonM
                    {
                        Id = item.ItemCatId,
                        Text = item.ItemCategoryDesc
                    }).ToList<IdTextJsonM>();
                ViewBag.itemCategoriesJsonStr = JsonSerializer.Serialize(itemCategoryList);

                return View(polyCombinationVMList);
            }
        }
    }

}
