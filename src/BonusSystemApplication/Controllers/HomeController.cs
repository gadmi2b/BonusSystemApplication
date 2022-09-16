using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;
using Microsoft.Data.SqlClient.Server;
using BonusSystemApplication.Models.ViewModels;
using BonusSystemApplication.Models.ViewModels.Index;
using BonusSystemApplication.Models.ViewModels.FormViewModel;

namespace BonusSystemApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IGenericRepository<User> userRepository;
        private IGenericRepository<Workproject> workprojectRepository;

        private IFormGlobalAccessRepository formGlobalAccessRepository;
        private IFormRepository formRepository;

        public HomeController(ILogger<HomeController> logger, IFormRepository formRepo, IGenericRepository<User> userRepo,
                              IGenericRepository<Workproject> workprojectRepo, IFormGlobalAccessRepository formGlobalAccessRepo)
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

            #region Queries to request available for User forms

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
            #region Validate selected form ids
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

            if(selectedFormIds.Count() == 0)
            {
                return RedirectToAction("Index");
            }
            #endregion

            long id = selectedFormIds.ElementAt(0);
            Form form = formRepository.GetForm(id);

            ObjectivesDefinition objectivesDefinition = null;
            ObjectivesSignature obectivesSignature = null;
            ResultsDefinition resultsDefinition = null;
            ResultsSignature resultsSignature = null;

            #region Fill ViewModels
            objectivesDefinition = new ObjectivesDefinition
            {
                FormId = form.Id,
                IsObjectivesFreezed = form.IsObjectivesFreezed,
                Period = form.Period.ToString(),
                Year = form.Year.ToString(),
                ObjectivesResults = form.ObjectivesResults,
                EmployeeFullName = $"{form.Employee.LastNameEng} {form.Employee.FirstNameEng}",
                ManagerFullName = $"{form.Manager?.LastNameEng} {form.Manager?.FirstNameEng}",
                ApproverFullName = $"{form.Approver?.LastNameEng} {form.Approver?.FirstNameEng}",
                WorkprojectName = form.Workproject.Name,
                IsWpmHox = form.IsWpmHox,
                Team = form.Employee.Team.Name,
                Position = form.Employee.Position.NameEng,
                Pid = form.Employee.Pid,
                WorkprojectDescription = form.Workproject.Name,
            };

            if (objectivesDefinition.IsObjectivesFreezed)
            {
                obectivesSignature = new ObjectivesSignature
                {
                    IsObjectivesSignedByEmployee = form.IsObjectivesSignedByEmployee,
                    ObjectivesEmployeeSignature = form.ObjectivesEmployeeSignature == null ? string.Empty : form.ObjectivesEmployeeSignature,
                    IsObjectivesRejectedByEmployee = form.IsObjectivesRejectedByEmployee,
                    IsObjectivesSignedByManager = form.IsObjectivesSignedByManager,
                    ObjectivesManagerSignature = form.ObjectivesManagerSignature == null ? string.Empty : form.ObjectivesManagerSignature,
                    IsObjectivesSignedByApprover = form.IsObjectivesSignedByApprover,
                    ObjectivesApproverSignature = form.ObjectivesApproverSignature == null ? string.Empty : form.ObjectivesApproverSignature,
                };

                if (obectivesSignature.IsObjectivesSigned)
                {
                    resultsDefinition = new ResultsDefinition
                    {
                        IsResultsFreezed = form.IsResultsFreezed,
                        OverallKpi = form.OverallKpi,
                        IsProposalForBonusPayment = form.IsProposalForBonusPayment,
                        ManagerComment = form.ManagerComment,
                        EmployeeComment = form.EmployeeComment,
                        OtherComment = form.OtherComment,
                        ObjectivesResults = form.ObjectivesResults,
                    };

                    if (resultsDefinition.IsResultsFreezed)
                    {
                        resultsSignature = new ResultsSignature()
                        {
                            IsResultsSignedByEmployee = form.IsResultsSignedByEmployee,
                            ResultsEmployeeSignature = form.ResultsEmployeeSignature == null ? string.Empty : form.ResultsEmployeeSignature,
                            IsResultsRejectedByEmployee = form.IsResultsRejectedByEmployee,
                            IsResultsSignedByManager = form.IsResultsSignedByManager,
                            ResultsManagerSignature = form.ResultsManagerSignature == null ? string.Empty : form.ResultsManagerSignature,
                            IsResultsSignedByApprover = form.IsResultsSignedByApprover,
                            ResultsApproverSignature = form.ResultsApproverSignature == null ? string.Empty : form.ResultsApproverSignature,
                        };

                        if (resultsSignature.IsResultsSigned)
                        {
                            //do nothing
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                    else
                    {
                        resultsSignature = new ResultsSignature();
                    }
                }
                else
                {
                    resultsDefinition = new ResultsDefinition();
                    resultsSignature = new ResultsSignature();
                }
            }
            else
            {
                obectivesSignature = new ObjectivesSignature();
                resultsDefinition = new ResultsDefinition();
                resultsSignature = new ResultsSignature();
            }
            #endregion

            // TODO: check form status:
            //       stage#1: only ObjectivesDefinition should be loaded
            //                only ObjectivesDefinition could be saved
            //       stage#2: only ObjectivesDefinition and ObjectivesSignature should be loaded
            //                only ObjectivesSignature could be saved
            //       stage#3: only ObjectivesDefinition and ObjectivesSignature and ResultsDefinition should be loaded
            //                only ResultsDefinition could be saved
            //       stage#4: only ObjectivesDefinition and ObjectivesSignature and ResultsDefinition and ResultsSignature should be loaded
            //                only ResultsSignature could be saved



            // ----------------------------------------------------------------------

            List<User> users = userRepository.GetQueryForAll()
                .Select(u => new User
                {
                    LastNameEng = u.LastNameEng,
                    FirstNameEng = u.FirstNameEng,
                })
                .ToList();

            List<Workproject> workprojects = workprojectRepository.GetQueryForAll()
                .Select(w => new Workproject
                {
                    Name= w.Name,
                })
                .ToList();

            // TODO generate selectLists in a new ViewModel

            //ViewBag.Users = userRepository.GetAll().Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.LastNameEng} {u.FirstNameEng}" }).ToList();
            //ViewBag.Workprojects = workprojectRepository.GetAll().Select(w => new SelectListItem { Value = w.Id.ToString(), Text = w.Name }).ToList();

            return View(form);
        }

        [HttpPost]
        public IActionResult CreateFormBasedOnSelection(List<long> selectedFormIds)
        {
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

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult OpenBlankForm()
        {
            //TODO: create blank form model: necessary ObjectiveResults and other
            Form form = new Form()
            {
                Id = 0,
            };

            List<ObjectiveResult> objectivesResults = new List<ObjectiveResult>();
            for(int i=0; i < 10; i++)
            {
                ObjectiveResult objectiveResult = new ObjectiveResult()
                {
                    Id = 0,
                    Row = i + 1,
                    Form = form,
                };
                objectivesResults.Add(objectiveResult);
            }

            form.ObjectivesResults = objectivesResults;

            return View("Form", form);
        }


        [HttpPost]
        public IActionResult PromoteFormsBasedOnSelection(List<long> selectedFormIds)
        {
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

            return RedirectToAction("Index");
        }

        [HttpPost]
        public void GetWorkprojectDescription([FromBody] string selectedData)
        {
            //JsonResult RetVal = new JsonResult();
            return;
        }
    }
}