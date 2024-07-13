using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDemo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ICompanyRespository _compRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IBonusRespository _bonRespo;

        public EmployeeController(ICompanyRespository comRepo, IEmployeeRepository empRepo, IBonusRespository bonRespo)
        {
            _compRepo = comRepo;
            _bonRespo = bonRespo;
            _empRepo = empRepo;
          
        }


        public async Task<IActionResult> Index(int CompanyId=0)
        {
            if(_empRepo.GetAll() != null)
            {
                List<Employee> employees = _bonRespo.GetEmployeeWithCompany(CompanyId);
                //List<Employee> employees = _empRepo.GetAll();
                //foreach(Employee obj in employees)
                //{
                //    obj.Company = _compRepo.Find(obj.CompanyId);
                //}
                return View(employees);
            }
            return View();           
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _compRepo.Find(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CompanyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = CompanyList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId, Name, Email,Phone,Title, CompanyId")] Employee employee)
        {
            IEnumerable<SelectListItem> CompanyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = CompanyList;
            if (ModelState.IsValid)
            {
                _empRepo.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = _empRepo.Find(id.GetValueOrDefault());
            IEnumerable<SelectListItem> CompanyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = CompanyList;

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("EmployeeId,Name, Email,Phone,Title,CompanyId")] Employee employee, int id)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> CompanyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });
            ViewBag.CompanyList = CompanyList;

            if (ModelState.IsValid)
            {
                _empRepo.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _empRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }
    }
}
