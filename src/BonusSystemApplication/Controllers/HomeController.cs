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
            // TODO: to rename FormGlobalAccess to UserGlobalAccess
            // TODO: to rename FormLocalAccess to UserLocalAccess
            

            // TODO: get userId during login process
            long userId = 5;


            #region Global accesses for user:
            IEnumerable<FormGlobalAccess> formGlobalAccesses = formGlobalAccessRepository.GetFormGlobalAccessByUserId(userId);
            #endregion

            #region Collect application User data information
            UserDataSinglton userData = new UserDataSinglton(userId, formGlobalAccesses);
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
            if (globalAccessFormsQuery != null)
            {
                combinedFormsQuery = combinedFormsQuery.Union(globalAccessFormsQuery);
            }
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

            FormDataSingleton formData = new FormDataSingleton(availableForms);

            #region Determine formIds with Global access
            List<long> formIdsWithGlobalAccess = new List<long>();

            foreach (FormGlobalAccess formGA in userData.FormGlobalAccesses)
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

            #region Determine formIds with individual participation
            Func<Form, bool> delEmployeeParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Employee);
            List<long> formIdsWithEmployeeParticipation = availableForms
                .Where(f => delEmployeeParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            Func<Form, bool> delManagerParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Manager);
            List<long> formIdsWithManagerParticipation = availableForms
                .Where(f => delManagerParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            Func<Form, bool> delApproverParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Approver);
            List<long> formIdsWithApproverParticipation = availableForms
                .Where(f => delApproverParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            #endregion

            // TODO: find and apply AutoMapper here
            // TODO: all operations for HomeIndexViewModel preparation should be outside controller

            #region Preparation of Table filters: collecting all available items
            //Access Filters
            List<Permissions> availablePermissions = new List<Permissions>();
            if (formIdsWithGlobalAccess.Count > 0)
            {
                availablePermissions.Add(Permissions.GlobalAccess);
            }
            if (formIdsWithLocalAccess.Count > 0)
            {
                availablePermissions.Add(Permissions.LocalAccess);
            }
            if (formIdsWithEmployeeParticipation.Count > 0)
            {
                availablePermissions.Add(Permissions.Employee);
            }
            if (formIdsWithManagerParticipation.Count > 0)
            {
                availablePermissions.Add(Permissions.Manager);
            }
            if (formIdsWithApproverParticipation.Count > 0)
            {
                availablePermissions.Add(Permissions.Approver);
            }

            //Employees Names
            List<string> availableEmployees = new List<string>();
            availableEmployees = availableForms
                .Select(f => ($"{f.Employee.LastNameEng} {f.Employee.FirstNameEng}"))
                .Distinct()
                .ToList();

            //Periods
            List<Periods> availablePeriods = new List<Periods>();
            availablePeriods = availableForms
                .Select(f => f.Period)
                .Distinct()
                .ToList();

            //Years
            List<int> availableYears = new List<int>();
            availableYears = availableForms
                .Select(f => f.Year)
                .Distinct()
                .ToList();

            //Departments
            List<string> availableDepartments = new List<string>();
            availableDepartments = availableForms
                .Select(f => f.Employee.Department.Name)
                .Distinct()
                .ToList();

            //Teams
            List<string> availableTeams = new List<string>();
            availableTeams = availableForms
                .Select(f => f.Employee.Team.Name)
                .Distinct()
                .ToList();

            //Workprojects
            List<string> availableWorkprojects = new List<string>();
            availableWorkprojects = availableForms
                .Select(f => f.Workproject.Name)
                .Distinct()
                .ToList();
            #endregion

            #region Preparation of Table filters: generating Select lists
            //tableFilters.EmployeeSelectList = new GenericMultiSelectList<string, SelectEmployee>(availableEmployees, null);
            //tableFilters.PeriodSelectList = new GenericMultiSelectList<Periods, SelectPeriod>(availablePeriods, null);
            //tableFilters.YearSelectList = new GenericMultiSelectList<int, SelectYear>(availableYears, null);
            //tableFilters.PermissionSelectList = new GenericMultiSelectList<Permissions, SelectPermission>(availablePermissions, null);
            //tableFilters.DepartmentSelectList = new GenericMultiSelectList<string, SelectDepartment>(availableDepartments, null);
            //tableFilters.TeamSelectList = new GenericMultiSelectList<string, SelectTeam>(availableTeams, null);
            //tableFilters.WorkprojectSelectList = new GenericMultiSelectList<string, SelectWorkproject>(availableWorkprojects, null);
            #endregion

            #region Checking of selected by user filters
            //TODO: selected filters should be in available items
            //if (!string.IsNullOrEmpty(tableFilters.Employee) &&
            //    !availableEmployees.Contains(tableFilters.Employee))
            //{
            //    tableFilters.Employee = string.Empty;
            //}
            //if (!string.IsNullOrEmpty(tableFilters.Period) &&
            //    Enum.TryParse(tableFilters.Period, out Periods resultPeriod) &&
            //    !availablePeriods.Contains(resultPeriod))
            //{
            //    tableFilters.Period = string.Empty;
            //}
            //if (!string.IsNullOrEmpty(tableFilters.Access) &&
            //    Enum.TryParse(tableFilters.Permission, out Permissions resultPermission) &&
            //    !availablePermissions.Contains(resultPermission))
            //{
            //    tableFilters.Access = string.Empty;
            //}
            //if (!string.IsNullOrEmpty(tableFilters.Year) &&
            //    int.TryParse(tableFilters.Year, out int resultYear) &&
            //    !availableYears.Contains(resultYear))
            //{
            //    tableFilters.Year = string.Empty;
            //}
            //if (!string.IsNullOrEmpty(tableFilters.Department) &&
            //    !availableDepartments.Contains(tableFilters.Department))
            //{
            //    tableFilters.Department = string.Empty;
            //}
            //if (!string.IsNullOrEmpty(tableFilters.Team) &&
            //    !availableTeams.Contains(tableFilters.Team))
            //{
            //    tableFilters.Team = string.Empty;
            //}
            //if (!string.IsNullOrEmpty(tableFilters.Workproject) &&
            //    !availableWorkprojects.Contains(tableFilters.Workproject))
            //{
            //    tableFilters.Workproject = string.Empty;
            //}
            #endregion

            #region Filtering in acc. to FormSelector object
            //tableFilters.Period = "Q4";

            //List<Permissions> permissions = new List<Permissions>();
            //List<TableRow> tableRows = availableForms
            //    .Where(f => userData.GetPermissions(f, out permissions) &&
            //                (string.IsNullOrEmpty(tableFilters.Employee) ? true : $"{f.Employee.LastNameEng} {f.Employee.FirstNameEng}" == tableFilters.Employee) &&
            //                (string.IsNullOrEmpty(tableFilters.Period) ? true : Enum.TryParse(tableFilters.Period, out resultPeriod) && f.Period == resultPeriod) &&
            //                (string.IsNullOrEmpty(tableFilters.Year) ? true : int.TryParse(tableFilters.Year, out int resultYear) && f.Year == resultYear) &&
            //                (string.IsNullOrEmpty(tableFilters.Department) ? true : f.Employee.Department.Name == tableFilters.Department) &&
            //                (string.IsNullOrEmpty(tableFilters.Team) ? true : f.Employee.Team.Name == tableFilters.Team) &&
            //                (string.IsNullOrEmpty(tableFilters.Permission) ? true : Enum.TryParse(tableFilters.Permissions, out Permissions resultPermission) && permissions.Contains(resultPermission)) &&
            //                (string.IsNullOrEmpty(tableFilters.Workproject) ? true : f.Workproject.Name == tableFilters.Workproject))
            //    .Select(f => new TableRow
            //    {
            //        Id = f.Id,
            //        EmployeeFullName = ($"{f.Employee.LastNameEng} {f.Employee.FirstNameEng}"),
            //        WorkprojectName = f.Workproject.Name,
            //        DepartmentName = f.Employee.Department?.Name,
            //        TeamName = f.Employee.Team?.Name,
            //        LastSavedDateTime = f.LastSavedDateTime,
            //        Period = f.Period,
            //        Year = f.Year,
            //        Permissions = permissions,
            //    })
            //    .ToList();
            #endregion

            #region prepare HomeIndexViewModel
            //HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel
            //{
            //    TableRows = tableRows,
            //    TableFilters = tableFilters
            //};
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