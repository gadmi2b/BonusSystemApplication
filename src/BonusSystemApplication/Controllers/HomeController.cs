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
            IEnumerable<FormGlobalAccess> formGlobalAccesses = formGlobalAccessRepository.GetFormGlobalAccessByUserId(userId);
            #endregion

            #region Forms where user has Global accesses:
            IQueryable<Form> globalAccessForms = formRepository.GetFormsWithGlobalAccess(formGlobalAccesses);
            // could be null here
            #endregion

            #region Forms where user has Participation:

            #endregion

            #region Forms where user has Local access:

            #endregion

            #region Request combination and data pulling into HomeIndexViewModels
            List<Form> availableForms = globalAccessForms.ToList();

            foreach (FormGlobalAccess formGA in formGlobalAccesses)
            {
                var expr = formRepository.GenerateGlobalAccessExpression(formGA).Compile();
                List<Form> query = availableForms.Where(expr).ToList();
                Console.WriteLine();
            }

            #endregion

            #region Filtering in according to incoming FormSelector object

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