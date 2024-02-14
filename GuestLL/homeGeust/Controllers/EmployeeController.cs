using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RelativeInfo.Repositores;
using guest.Models.Employee;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace GuestInfo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        //private readonly IWebHostEnvironment WebHostEnvironment;
        //IEmployeeRepository _iEmployeesRepository;
       // EmployeeRepository _iEmployeesRepository;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment _webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = _webHostEnvironment;
             
    }

        private string UploadedFile(IFormFile file)
        {
            try
            {
                string uniqueFileName = null;

                if (file != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public ActionResult Index()
        {
            //ViewData["User_Id"] = HttpContext.Session.GetString("User_Id");
           // ViewData["User_Name"] = HttpContext.Session.GetString("User_Name");

            List<Employee> users =
            [
                //add code of get data
                new Employee(),
            ];

            EmployeeRepository employeeRepository = new EmployeeRepository(_configuration);
            ModelState.Clear();
            //return View(umdb.ListAll());

            return View(employeeRepository.ListAll(Convert.ToInt64(ViewData["User_Id"])));
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {
            try
            {
                string uniqueFileName = UploadedFile(form.Files["ProfileImage"]);

                Employee model = new Employee();
                model.Name = form.ContainsKey("Name") ? form["Name"].First() : null;
                model.Address = form.ContainsKey("Address") ? form["Address"].First() : null;
              
                model.ImagePath = uniqueFileName;

                if (ModelState.IsValid)
                {

                    EmployeeRepository employeeRepository = new EmployeeRepository(_configuration);
                   if( employeeRepository.AddEmployee(model));


                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

            }
            
            return View();
        }



    }
}
