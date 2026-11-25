using Costing_Formula___KSPA.Models.ViewModels;
using Costing_Formula___KSPA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Costing_Formula___KSPA.Data;
using Costing_Formula___KSPA.Models.JsonModels;
using System.Text.Json;

namespace Costing_Formula___KSPA.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            IEnumerable<Customer> customersList = _db.Customers;

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            // Filling Customer types
            foreach (var customer in customersList)
            {
                // Filling Customer types
                customer.CustomerType = _db.CustomerTypes.FirstOrDefault(
                        record => record.CustomerTypeId == customer.CustomerTypeId
                    );

                // Filling Currency
                customer.Currency = _db.Currencies.FirstOrDefault(
                        record => record.CurrencyId == customer.CurrencyId
                    );

                // Filling Sales Rep
                customer.SRep = _db.SRepsOrCoordinators.FirstOrDefault(
                        record => record.SrocId == customer.SRepId
                    );

                // Filling Sales Coordinator
                customer.SCoordinator = _db.SRepsOrCoordinators.FirstOrDefault(
                        record => record.SrocId == customer.SCoordinatorId
                    );

                // Filling Company
                customer.Company = _db.Companies.FirstOrDefault(
                        record => record.CompanyId == customer.CompanyId
                    );
            }

            return View(customersList);
        }

        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            // Getting View Model CustomerVM
            CustomerVM customerVM = new CustomerVM()
            {
                Customer = new Customer(),
                CustomerTypeDropDown = _db.CustomerTypes.Select(item => new SelectListItem
                {
                    Text = item.CustomerTypeDesc,
                    Value = item.CustomerTypeId.ToString()
                }),
                SRepDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                }),
                SCoordinatorDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                }),
                CurrencyDropDown = _db.Currencies.Select(item => new SelectListItem
                {
                    Text = item.CurrencyCode,
                    Value = item.CurrencyId.ToString()
                }),
                CompanyDropDown = _db.Companies.Select(item => new SelectListItem
                {
                    Text = item.CompanyName,
                    Value = item.CompanyId.ToString()
                })
            };
            return View(customerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerVM customerVM)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            // For RepOrCoordinators redirect to relevent index page
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Index");

            if (customerVM == null || customerVM.Customer == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Customers.Add(customerVM.Customer);
                ViewBag.ErrorMessage = "Customer created successfully!";
                _db.SaveChanges();
            } else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            // To send View(completeObject). Otherwise selectLists will not fill
            customerVM.CustomerTypeDropDown = _db.CustomerTypes.Select(item => new SelectListItem
            {
                Text = item.CustomerTypeDesc,
                Value = item.CustomerTypeId.ToString()
            });
            customerVM.SRepDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
            {
                Text = item.SrocName,
                Value = item.SrocId.ToString()
            });
            customerVM.SCoordinatorDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
            {
                Text = item.SrocName,
                Value = item.SrocId.ToString()
            });
            customerVM.CurrencyDropDown = _db.Currencies.Select(item => new SelectListItem //May change to Currency View Model
            {
                Text = item.CurrencyCode,
                Value = item.CurrencyId.ToString()
            });
            customerVM.CompanyDropDown = _db.Companies.Select(item => new SelectListItem //May change to Currency View Model
            {
                Text = item.CompanyName,
                Value = item.CompanyId.ToString()
            });

            return View(customerVM);
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
            var customer = _db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerVM = new CustomerVM()
            {
                Customer = customer,
                CustomerTypeDropDown = _db.CustomerTypes.Select(item => new SelectListItem
                {
                    Text = item.CustomerTypeDesc,
                    Value = item.CustomerTypeId.ToString()
                }),
                SRepDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                }),
                SCoordinatorDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                }),
                CurrencyDropDown = _db.Currencies.Select(item => new SelectListItem
                {
                    Text = item.CurrencyCode,
                    Value = item.CurrencyId.ToString()
                }),
                CompanyDropDown = _db.Companies.Select(item => new SelectListItem
                {
                    Text = item.CompanyName,
                    Value = item.CompanyId.ToString()
                })
            };
            return View(customerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100000000)]
        [DisableRequestSizeLimit]
        public IActionResult Update(CustomerVM customerVM)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (customerVM == null || customerVM.Customer == null || customerVM.Customer.CustomerId == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Customers.Update(customerVM.Customer);
                TempData["SuccessMessage"] = "Customer updated successfully!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

                // Filling Customer Type
                customerVM.CustomerTypeDropDown = _db.CustomerTypes.Select(item => new SelectListItem
                {
                    Text = item.CustomerTypeDesc,
                    Value = item.CustomerTypeId.ToString()
                });
                // Filling Sales Rep
                customerVM.SRepDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                });
                // Filling Sales Coordinator
                customerVM.SCoordinatorDropDown = _db.SRepsOrCoordinators.Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                });
                // Filling Currency
                customerVM.CurrencyDropDown = _db.Currencies.Select(item => new SelectListItem
                {
                    Text = item.CurrencyCode,
                    Value = item.CurrencyId.ToString()
                });
                // Filling Company
                customerVM.CompanyDropDown = _db.Companies.Select(item => new SelectListItem
                {
                    Text = item.CompanyName,
                    Value = item.CompanyId.ToString()
                });
                return View(customerVM);
            }
        }

        public IActionResult ImportFromCsv()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            List<CustomerVM> customerVMList = new List<CustomerVM>();
            customerVMList.Add(new CustomerVM()
            {
                Customer = new Customer()
                {
                    CustomerTypeId = 1,
                    CurrencyId = 1,
                    CompanyId = 1,
                    CustomerShortCode = "PH",
                    CustomerName = "PH",
                    CustomerAddress = "PH",
                    Email = "PH",
                    SRepId = 1,
                    SCoordinatorId = 1,
                    IsActive = false
                },
                CustomerTypeDropDown = _db.CustomerTypes.Select(item => new SelectListItem
                {
                    Text = item.CustomerTypeDesc,
                    Value = item.CustomerTypeId.ToString()
                }),
                SRepDropDown = _db.SRepsOrCoordinators.Where(record =>
                    record.IsActive == true
                    &&
                    record.IsReprecentative == true
                ).Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                }),
                SCoordinatorDropDown = _db.SRepsOrCoordinators.Where(record =>
                    record.IsActive == true
                    &&
                    record.IsCoordinator == true
                ).Select(item => new SelectListItem
                {
                    Text = item.SrocName,
                    Value = item.SrocId.ToString()
                }),
                CurrencyDropDown = _db.Currencies.Select(item => new SelectListItem
                {
                    Text = item.CurrencyCode,
                    Value = item.CurrencyId.ToString()
                }),
                CompanyDropDown = _db.Companies.Select(item => new SelectListItem
                {
                    Text = item.CompanyName,
                    Value = item.CompanyId.ToString()
                })
            }); // This is a little hack

            var customerTypeList = _db.CustomerTypes
                .Where(record =>
                    record.IsActive == true
                ).Select(item => new IdTextJsonM
                {
                    Id = item.CustomerTypeId,
                    Text = item.CustomerTypeDesc
                }).ToList<IdTextJsonM>();
            ViewBag.CustomerTypeJsonStr = JsonSerializer.Serialize(customerTypeList);

            var currencyList = _db.Currencies
                .Where(record =>
                    record.IsActive == true
                ).Select(item => new IdTextJsonM
                {
                    Id = item.CurrencyId,
                    Text = item.CurrencyCode
                }).ToList<IdTextJsonM>();
            ViewBag.CurrencyJsonStr = JsonSerializer.Serialize(currencyList);

            var salesRepList = _db.SRepsOrCoordinators
                .Where(record =>
                    record.IsActive == true
                    &&
                    record.IsReprecentative == true
                ).Select(item => new IdTextJsonM
                {
                    Id = item.SrocId,
                    Text = item.SrocName
                }).ToList<IdTextJsonM>();
            ViewBag.SalesRepJsonStr = JsonSerializer.Serialize(salesRepList);

            var salesCoordList = _db.SRepsOrCoordinators
                .Where(record =>
                    record.IsActive == true
                    &&
                    record.IsCoordinator == true
                ).Select(item => new IdTextJsonM
                {
                    Id = item.SrocId,
                    Text = item.SrocName
                }).ToList<IdTextJsonM>();
            ViewBag.SalesCoordJsonStr = JsonSerializer.Serialize(salesCoordList);

            var companyList = _db.Companies
                .Where(record =>
                    record.IsActive == true
                ).Select(item => new IdTextJsonM
                {
                    Id = item.CompanyId,
                    Text = item.CompanyName
                }).ToList<IdTextJsonM>();
            ViewBag.CompanyJsonStr = JsonSerializer.Serialize(companyList);

            return View(customerVMList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportFromCsv(List<CustomerVM> customerVMList)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (!User.IsInRole(Costing_Formula___KSPA.Utility.Helper.Admin))
                return RedirectToAction("Forbidden", "Home");

            if (customerVMList == null)
            {
                return View(customerVMList);
            }

            List<Customer> customerList = new List<Customer>();
            foreach (var custoemrVM in customerVMList)
            {
                customerList.Add(custoemrVM.Customer);
            }

            if (customerList.Count == 0)
            {
                return View(customerVMList);
            }

            if (ModelState.IsValid)
            {
                _db.Customers.AddRange(customerList);
                TempData["SuccessMessage"] = "Import successful!";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                ViewBag.ErrorMessage = "There are few data entry errors. Please, fix them and retry.";

            return View(customerVMList);
        }
    }
}
