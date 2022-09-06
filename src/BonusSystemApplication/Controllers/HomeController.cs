using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;
using Microsoft.Data.SqlClient.Server;
using BonusSystemApplication.Models.ViewModels.Index;
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

        public IActionResult Index(TableFilters tableFilters)
        {
            // TODO: get userId during login process
            UserData.UserId = 5;

            #region Global accesses for user
            IEnumerable<FormGlobalAccess> formGlobalAccesses = formGlobalAccessRepository.GetFormGlobalAccessByUserId(UserData.UserId);
            #endregion

            #region Queries to request available for User forms (repeat in Form action)

                #region Queries for forms where user has Global accesses
                IQueryable<Form> globalAccessFormsQuery = formRepository.GetFormsWithGlobalAccess(formGlobalAccesses);
                #endregion

                #region Queries for forms where user has Local access
                IQueryable<Form> localAccessFormsQuery = formRepository.GetFormsWithLocalAccess(UserData.UserId);
                #endregion

                #region Queries for forms where user has Participation
                IQueryable<Form> participantFormsQuery = formRepository.GetFormsWithParticipation(UserData.UserId);
                #endregion

                #region Queries combination
                IQueryable<Form> combinedFormsQuery = participantFormsQuery.Union(localAccessFormsQuery);
                if (globalAccessFormsQuery != null)
                {
                    combinedFormsQuery = combinedFormsQuery.Union(globalAccessFormsQuery);
                }
            #endregion

            #endregion

            #region Load data from database into forms
            List<Form> availableForms = combinedFormsQuery
                .Select(f => new Form {
                    Id = f.Id,
                    Employee = f.Employee,
                    Workproject = f.Workproject,
                    FormLocalAccess = f.FormLocalAccess,
                    ApproverId = f.ApproverId,
                    ManagerId = f.ManagerId,
                    WorkprojectId = f.WorkprojectId,
                    LastSavedDateTime = f.LastSavedDateTime,
                    Period = f.Period,
                    Year = f.Year,
                })
                .ToList();
            #endregion

            FormDataSingleton formData = new FormDataSingleton(availableForms, formGlobalAccesses);

            tableFilters.SelectedPeriods.Add("Q2");
            tableFilters.SelectedPeriods.Add("Q1");
            tableFilters.SelectedPeriods.Add("Q5");
            tableFilters.SelectedPeriods.Add("Q3");
            tableFilters.SelectedPeriods.Add("");


            tableFilters.PrepareMultiSelectLists(formData);

            List<string> formPermissions = new List<string>();
            List<TableRow> tableRows = availableForms
                .Where(f => formData.IsFormHasPermissions(f, out formPermissions) && tableFilters.IsFormCanBeShown(f, formPermissions))
                .Select(f => new TableRow
                {
                    Id = f.Id,
                    EmployeeFullName = ($"{f.Employee.LastNameEng} {f.Employee.FirstNameEng}"),
                    WorkprojectName = f.Workproject.Name,
                    DepartmentName = f.Employee.Department?.Name,
                    TeamName = f.Employee.Team?.Name,
                    LastSavedDateTime = f.LastSavedDateTime,
                    Period = f.Period.ToString(),
                    Year = f.Year.ToString(),
                    Permissions = formPermissions,
                })
                .ToList();

            #region prepare HomeIndexViewModel
            HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel
            {
                TableRows = tableRows,
                TableFilters = tableFilters
            };
            #endregion

            return View();
        }

        public IActionResult Form()
        {
            // TODO: to rework
            //       id should come from Index view
            //       add check that this form is available for current user
            //       no view bags - all information in ViewModel
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