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

            UserData.UserId = 5;
        }

        public IActionResult Index(UserSelections userSelections)
        {
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

            UserData.SetAvailableFormIds(availableForms);
            FormDataAvailable formDataAvailable = new FormDataAvailable(availableForms, UserData.UserId, formGlobalAccesses);
            userSelections.PrepareSelections(formDataAvailable);
            FormDataSorted formDataSorted = new FormDataSorted(availableForms, UserData.UserId, formGlobalAccesses, userSelections);

            #region Prepare TableRows
            List<TableRow> tableRows = formDataSorted.FormAndPermissions
                .Select(pair => new TableRow
                {
                    Id = pair.Key.Id,
                    EmployeeFullName = ($"{pair.Key.Employee.LastNameEng} {pair.Key.Employee.FirstNameEng}"),
                    WorkprojectName = pair.Key.Workproject.Name,
                    DepartmentName = pair.Key.Employee.Department?.Name,
                    TeamName = pair.Key.Employee.Team?.Name,
                    LastSavedDateTime = pair.Key.LastSavedDateTime,
                    Period = pair.Key.Period.ToString(),
                    Year = pair.Key.Year.ToString(),
                    Permissions = pair.Value
                })
                .ToList();
            #endregion

            #region Prepare TableSelectLists
            TableSelectLists tableSelectLists = new TableSelectLists();
            tableSelectLists.PrepareMultiSelectLists(formDataSorted, userSelections);
            #endregion

            #region Prepare HomeIndexViewModel
            HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel
            {
                TableRows = tableRows,
                TableSelectLists = tableSelectLists,
            };
            #endregion

            return View(homeIndexViewModel);
        }

        [HttpPost]
        public IActionResult Form(List<long> selectedFormIds)
        {
            //TODO: JS should in a cycle calls Form action and send to it only one select formId
            #region Validation of selected form ids
            List<long> itemsToRemove = new List<long>();
            foreach (long formId in selectedFormIds)
            {
                if (formId <= 0 || !UserData.availableFormIds.Contains(formId))
                {
                    itemsToRemove.Add(formId);
                }
            }
            selectedFormIds.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();
            #endregion

            long id = selectedFormIds.ElementAt(0);

            IEnumerable<Form> forms = formRepository.GetForm(id);
            Form form = forms.First();

            ViewBag.Users = userRepository.GetAll().Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.LastNameEng} {u.FirstNameEng}" }).ToList();
            ViewBag.Workprojects = workprojectRepository.GetAll().Select(w => new SelectListItem { Value = w.Id.ToString(), Text = w.Name }).ToList();

            return View(form);
        }

        [HttpPost]
        public IActionResult OpenBlankForm()
        {
            //TODO: create blank form model: necessary ObjectiveResults and other
            Form form = new Form();
            return View("Form", form);
        }

        [HttpPost]
        public void GetWorkprojectDescription([FromBody] string selectedData)
        {
            //JsonResult RetVal = new JsonResult();
            return;
        }
    }
}