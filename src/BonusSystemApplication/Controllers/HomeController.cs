using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;
using BonusSystemApplication.Models.ViewModels;

namespace BonusSystemApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Workroject> workprojectRepository;

        private IFormGlobalAccessRepository formGlobalAccessRepository;
        private IFormRepository formRepository;

        public HomeController(ILogger<HomeController> logger, IFormRepository formRepo, IGenericRepository<User> userRepo,
                                IGenericRepository<Workroject> workprojectRepo, IFormGlobalAccessRepository formGlobalAccessRepo)
        {
            _logger = logger;
            formRepository = formRepo;
            userRepository = userRepo;
            workprojectRepository = workprojectRepo;
            formGlobalAccessRepository = formGlobalAccessRepo;
        }

        public IActionResult Index()
        {
            // TODO: get userId during login process
            long userId = 8;

            // TODO: to fill accessFilters after getting all available forms
            List<AccessFilter> accessFilters = new List<AccessFilter>();

            #region Global accesses for user:
            IEnumerable<FormGlobalAccess> formGlobalAccess = formGlobalAccessRepository.GetFormGlobalAccessByUserId(userId);
            #endregion

            #region Forms where user has Global accesses:
            IQueryable<Form> allForms = formRepository.GetFormsWithGlobalAccess(formGlobalAccess);
            #endregion

            return View();
        }

        public IActionResult Form()
        {
            long id = 1;

            IEnumerable<Form> forms = formRepository.GetForm(id);
            Form form = forms.First();


            ViewBag.Users = userRepository.GetAll().Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.LastNameEng} {u.FirstNameEng}" }).ToList();
            ViewBag.Workprojects = workprojectRepository.GetAll().Select(w => new SelectListItem { Value = w.Id.ToString(), Text = w.Name }).ToList();

            return View(form);
        }

        [HttpPost]
        public void GetWorkprojectDescription([FromBody] string selectedData)
        {
            //JsonResult RetVal = new JsonResult();
            return;
        }
    }
}