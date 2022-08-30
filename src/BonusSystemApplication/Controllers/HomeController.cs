using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;
using BonusSystemApplication.Models.ViewModels;
using Microsoft.Data.SqlClient.Server;

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
            long userId = 7;

            #region Global accesses for user:
            IEnumerable<FormGlobalAccess> formGlobalAccesses = formGlobalAccessRepository.GetFormGlobalAccessByUserId(userId);
            #endregion

            #region Queries for forms where user has Global accesses:
            IQueryable<Form> globalAccessFormsQuery = formRepository.GetFormsWithGlobalAccess(formGlobalAccesses);
            #endregion

            #region Queries for forms where user has Local access:
            IQueryable<Form> localAccessFormsQuery = formRepository.GetFormsWithLocalAccess(userId);
            #endregion

            #region Queries for forms where user has Participation:
            IQueryable<Form> participantFormsQuery = formRepository.GetFormsWithParticipation(userId);
            #endregion

            #region Queries combination
            IQueryable<Form> combinedFormsQuery = participantFormsQuery.Union(localAccessFormsQuery);
            if(globalAccessFormsQuery != null)
            {
                combinedFormsQuery = combinedFormsQuery.Union(globalAccessFormsQuery);
            }
            #endregion

            #region Request data from database into forms
            List<Form> availableForms = combinedFormsQuery
                .Select(f => new Form {
                    Id = f.Id,
                    Employee= f.Employee,
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

            #region Determine formIds with Global access
            List<long> formIdsWithGlobalAccess = new List<long>();

            foreach (FormGlobalAccess formGA in formGlobalAccesses)
            {
                Func<Form, bool> delegateGA = ExpressionBuilder.GetExpressionForGlobalAccess(formGA).Compile();
                formIdsWithGlobalAccess = availableForms
                    .Where(f => delegateGA.Invoke(f))
                    .Select(f => f.Id)
                    .ToList();
            }
            #endregion

            #region Determine formIds with Local access
            Func<Form, bool> delegateLA = ExpressionBuilder.GetExpressionForLocalAccess(userId).Compile();
            List<long> formIdsWithLocalAccess = availableForms
                .Where(f => delegateLA.Invoke(f))
                .Select(f => f.Id)
                .ToList();
            #endregion

            #region Determine formIds with Participation
            // Common participation is not used. It was splitted into Employee/Manager/Approver
            //
            //Func<Form, bool> delegateParticipation = ExpressionBuilder.GetExpressionForParticipation(userId).Compile();
            //List<long> formIdsWithParticipation = availableForms
            //    .Where(f => delegateParticipation.Invoke(f))
            //    .Select(f => f.Id)
            //    .ToList();

            Func<Form, bool> delEmployeeParticipation = ExpressionBuilder.GetMethodForParticipation(userId, AccessFilter.Employee);
            List<long> formIdsWithEmployeeParticipation = availableForms
                .Where(f => delEmployeeParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            Func<Form, bool> delManagerParticipation = ExpressionBuilder.GetMethodForParticipation(userId, AccessFilter.Manager);
            List<long> formIdsWithManagerParticipation = availableForms
                .Where(f => delManagerParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            Func<Form, bool> delApproverParticipation = ExpressionBuilder.GetMethodForParticipation(userId, AccessFilter.Approver);
            List<long> formIdsWithApproverParticipation = availableForms
                .Where(f => delApproverParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            #endregion

            // TODO: find and apply AutoMapper here
            // TODO: integrate AccessFilters identification into viewModels object creation
            #region ViewModels preparation:
            List<HomeIndexViewModel> homeIndexViewModels = availableForms
                .Select(f => new HomeIndexViewModel
                {
                    Id = f.Id,
                    EmployeeFullName = f.Employee.LastNameEng + " " + f.Employee.FirstNameEng,
                    WorkprojectName = f.Workproject.Name,
                    LastSavedDateTime = f.LastSavedDateTime,
                    Period = f.Period,
                    Year = f.Year,
                })
                .ToList();

            HomeIndexViewModel.IdentifyAccessFilters(homeIndexViewModels,
                                                     formIdsWithGlobalAccess,
                                                     formIdsWithLocalAccess,
                                                     formIdsWithEmployeeParticipation,
                                                     formIdsWithManagerParticipation,
                                                     formIdsWithApproverParticipation);
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