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

            UserData.UserId = 7;
        }

        public IActionResult Index(UserSelections userSelections)
        {
            #region Global accesses for user
            IEnumerable<FormGlobalAccess> formGlobalAccesses = formGlobalAccessRepository.GetFormGlobalAccessByUserId(UserData.UserId);
            #endregion

            #region Queries to request available for User forms

                #region Query for forms where user has Global accesses
                IQueryable<Form> globalAccessFormsQuery = formRepository.GetFormsWithGlobalAccess(formGlobalAccesses);
                #endregion

                #region Query for forms where user has Local access
                IQueryable<Form> localAccessFormsQuery = formRepository.GetFormsWithLocalAccess(UserData.UserId);
                #endregion

                #region Query for forms where user has Participation
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
            TableSelectLists tableSelectLists = new TableSelectLists(formDataAvailable, formDataSorted, userSelections);
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
            //TODO: selectedFormIds.Count() = 0 means blank form should be opened
            //                                  empty Form should be created
            //      selectedFormIds.Count() > 0 means validate selected form ids
            //                                  formId should be found in the DB

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

            // selection of ONE formId is already controlled on client side
            // control here - just for double check
            long id = selectedFormIds.ElementAt(0);

            #region Getting formQuery and Form data precisely
            IQueryable<Form> formQuery = formRepository.GetFormQuery(id);
            Form form = formQuery //TODO: add EmployeeId, ManagerId, ApproverId
                .Select(f => new Form
                {
                    // ObjectivesDefinition data block
                    Id = f.Id,
                    IsObjectivesFreezed = f.IsObjectivesFreezed,
                    Employee = new User
                    {
                        Id = f.Employee.Id,
                        FirstNameEng = f.Employee.FirstNameEng,
                        LastNameEng = f.Employee.LastNameEng,
                        Pid = f.Employee.Pid,
                        Team = new Team
                        {
                            Name = f.Employee.Team == null ? string.Empty : f.Employee.Team.Name,
                        },
                        Position = new Position
                        {
                            NameEng = f.Employee.Position == null ? string.Empty : f.Employee.Position.NameEng,
                        },
                    },
                    Manager = new User
                    {
                        Id = f.ManagerId == null ? 0 : (long)f.ManagerId,
                        FirstNameEng = f.Employee.FirstNameEng,
                        LastNameEng = f.Employee.LastNameEng,
                    },
                    Approver = new User
                    {
                        Id = f.ApproverId == null ? 0 : (long)f.ApproverId,
                        FirstNameEng = f.Employee.FirstNameEng,
                        LastNameEng = f.Employee.LastNameEng,
                    },
                    Workproject = new Workproject
                    {
                        Id = f.WorkprojectId == null ? 0 : (long)f.WorkprojectId,
                        Name = f.Workproject == null ? string.Empty : f.Workproject.Name,
                        Description = f.Workproject == null ? string.Empty : f.Workproject.Description,
                    },
                    Period = f.Period,
                    Year = f.Year,
                    IsWpmHox = f.IsWpmHox,
                    ObjectivesResults = f.ObjectivesResults,

                    // ObjectivesSignature data block
                    IsObjectivesSignedByEmployee = f.IsObjectivesSignedByEmployee,
                    ObjectivesEmployeeSignature = f.ObjectivesEmployeeSignature,
                    IsObjectivesRejectedByEmployee = f.IsObjectivesRejectedByEmployee,
                    IsObjectivesSignedByManager = f.IsObjectivesSignedByManager,
                    ObjectivesManagerSignature = f.ObjectivesManagerSignature,
                    IsObjectivesSignedByApprover = f.IsObjectivesSignedByApprover,
                    ObjectivesApproverSignature = f.ObjectivesApproverSignature,

                    // ResultsDefinition data block
                    IsResultsFreezed = f.IsResultsFreezed,
                    OverallKpi = f.OverallKpi,
                    IsProposalForBonusPayment = f.IsProposalForBonusPayment,
                    ManagerComment = f.ManagerComment,
                    EmployeeComment = f.EmployeeComment,
                    OtherComment = f.OtherComment,

                    // ResultsSignature data block
                    IsResultsSignedByEmployee = f.IsResultsSignedByEmployee,
                    ResultsEmployeeSignature = f.ResultsEmployeeSignature,
                    IsResultsRejectedByEmployee = f.IsResultsRejectedByEmployee,
                    IsResultsSignedByManager = f.IsResultsSignedByManager,
                    ResultsManagerSignature = f.ResultsManagerSignature,
                    IsResultsSignedByApprover = f.IsResultsSignedByApprover,
                    ResultsApproverSignature = f.ResultsApproverSignature,
                })
                .First();
            #endregion

            #region Getting queries for Users and Workprojects
            IQueryable<User> usersQuery = userRepository.GetQueryForAll();
            IQueryable<Workproject> workprojectsQuery = workprojectRepository.GetQueryForAll();
            #endregion

            #region Prepare HomeFormViewModel
            HomeFormViewModel homeFormViewModel = new HomeFormViewModel
            {
                ObjectivesDefinition = new ObjectivesDefinition(form),
                ObjectivesSignature = new ObjectivesSignature(form),
                ResultsDefinition = new ResultsDefinition(form),
                ResultsSignature = new ResultsSignature(form),

                PeriodSelectList = Enum.GetNames(typeof(Periods))
                    .Select(s => new SelectListItem
                    {
                        Value = s,
                        Text = s,
                    })
                    .ToList(),
                EmployeeSelectList = usersQuery
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id.ToString(),
                        Text = $"{u.LastNameEng} {u.FirstNameEng}",
                    })
                    .ToList(),
                WorkprojectSelectList = workprojectsQuery
                    .Select(w => new SelectListItem
                    {
                        Value = w.Id.ToString(),
                        Text = w.Name,
                    })
                    .ToList(),
            };
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


            //ViewBag.Users = userRepository.GetAll().Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.LastNameEng} {u.FirstNameEng}" }).ToList();
            //ViewBag.Workprojects = workprojectRepository.GetAll().Select(w => new SelectListItem { Value = w.Id.ToString(), Text = w.Name }).ToList();

            return View(homeFormViewModel);
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

            #region Creation of a new form based on selection
                //form id should be equal to 0
                //only Objectives should be included
                //other fields - default

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

        [HttpPost]
        public IActionResult SaveForm(ObjectivesDefinition objectivesDefinition,
                                      ObjectivesSignature objectivesSignature,
                                      ResultsDefinition resultsDefinition,
                                      ResultsSignature resultsSignature)
        {
            
            


            return RedirectToAction("Form");
        }
    }
}