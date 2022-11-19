using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;
using Microsoft.Data.SqlClient.Server;
using BonusSystemApplication.Models.ViewModels;
using BonusSystemApplication.Models.ViewModels.Index;
using BonusSystemApplication.Models.ViewModels.FormViewModel;
using System.Text.Json;


//using Newtonsoft.Json.Serialization;

namespace BonusSystemApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IGenericRepository<User> userGenRepository;
        private IGenericRepository<Workproject> workprojectGenRepository;

        private IFormGlobalAccessRepository formGlobalAccessRepository;
        private IFormRepository formRepository;
        private IWorkprojectRepository workprojectRepository;
        private IUserRepository userRepository;


        public HomeController(ILogger<HomeController> logger,
                              IGenericRepository<User> userGenRepo,
                              IGenericRepository<Workproject> workprojectGenRepo,
                              IFormGlobalAccessRepository formGlobalAccessRepo,
                              IFormRepository formRepo,
                              IWorkprojectRepository workprojectRepo,
                              IUserRepository userRepo)
        {
            _logger = logger;
            formRepository = formRepo;
            userGenRepository = userGenRepo;
            workprojectGenRepository = workprojectGenRepo;
            formGlobalAccessRepository = formGlobalAccessRepo;
            workprojectRepository = workprojectRepo;
            userRepository = userRepo;

            UserData.UserId = 9;
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
            TableSelectLists tableSelectLists = new TableSelectLists(formDataAvailable, userSelections);
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

        [Route("Home/Form/{id:long}")]
        public IActionResult Form(long id)
        {
            #region Form preparing depending on selected Id
            Form form = null;

            if (id == 0)
            {
                form = new Form()
                {
                    Id = 0,
                };

                List<ObjectiveResult> objectivesResults = new List<ObjectiveResult>();
                for (int i = 0; i < 10; i++)
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
            }
            else
            {
                #region Validate selected form id
                if (id < 0 || !UserData.availableFormIds.Contains(id))
                {
                    // TODO: incorrect formId was requested to be opened
                    //       to add error page to show it to user

                    RedirectToAction("Index");
                }
                #endregion

                #region Getting Form data precisely
                form = formRepository.GetFormData(id);
                #endregion
            }
            #endregion

            #region Getting queries for Users and Workprojects
            IQueryable<User> usersQuery = userGenRepository.GetQueryForAll();
            IQueryable<Workproject> workprojectsQuery = workprojectGenRepository.GetQueryForAll();
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

            return View(homeFormViewModel);
        }

        [HttpPost]
        public IActionResult OpenBlankForm()
        {
            return RedirectToAction("Form", "Home", new {id = 0});
        }
        
        [HttpPost]
        public IActionResult CreateFormBasedOnSelection(List<long> selectedFormIds)
        {
            #region Validation of selected form id
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

            if (selectedFormIds.Count() == 0)
            {
                return RedirectToAction("Index");
            }

            #endregion

            #region Creation of a new form based on selected one
            //new form id should be equal to 0
            //only Objectives should be included
            //other fields = default values

            #endregion

            return RedirectToAction("Form", "Home", new { id = 0 });
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

            if (selectedFormIds.Count() == 0)
            {
                return RedirectToAction("Index");
            }
            #endregion

            #region Promote selected forms to a new forms
            // create identical forms with same definition, objectives and next period
            // other fields = default values
            // save them to DB
            #endregion

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSelectedForms(List<long> selectedFormIds)
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

            if (selectedFormIds.Count() == 0)
            {
                return RedirectToAction("Index");
            }
            #endregion

            // TODO: delete forms with selected ids
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action for Ajax request method requestWorkprojectDescription
        /// </summary>
        /// <param name="workprojectId">selected workproject id</param>
        /// <returns>status, message and workprojectDescription</returns>
        [HttpGet]
        public JsonResult GetWorkprojectDescription(long workprojectId)
        {
            #region check requested id
            if(workprojectId <= 0)
            {
                // TODO: to add to log: "Requested Id is less or equal to zero"
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad id was requested"
                });
            }
            #endregion

            Workproject workproject = workprojectRepository.GetWorkprojectData(workprojectId);
            string workprojectDescription = workproject?.Description == null ? string.Empty : workproject.Description;

            return new JsonResult(new
            {
                status = "success",
                message = "Operation was complited successfully",
                workprojectDescription = workprojectDescription,
            });
        }

        /// <summary>
        /// Action for Ajax request method requestEmployeeData
        /// </summary>
        /// <param name="employeeId">selected employee id</param>
        /// <returns>status, message, teamName, positionName and pid of employee</returns>
        [HttpGet]
        public JsonResult GetEmployeeData(long employeeId)
        {
            #region check requested id
            if (employeeId <= 0)
            {
                // TODO: to add to log: "Requested Id is less or equal to zero"
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad id was requested"
                });
            }
            #endregion
            User user = userRepository.GetUserData(employeeId);
            string teamName = user?.Team?.Name == null ? string.Empty : user.Team.Name;
            string positionName = user?.Position?.NameEng == null ? string.Empty : user.Position.NameEng;
            string pid = user?.Pid == null ? string.Empty : user.Pid;

            return new JsonResult(new
            {
                status = "success",
                message = "Operation was complited successfully",
                employeeTeam = teamName,
                employeePosition = positionName,
                employeePid = pid,
            });
        }

        [HttpGet]
        public void ChangeState(long formId)
        {
            // TODO: get current formId and user from Session
            //       check that user has permission to modify form

            // form should be saved

            // to find form by id
            // extract all states and all signatures into new Form object
            // invert state and remove corrsponding signatues
            #region Get form object with id, states and signatues

            #endregion

            #region 123

            #endregion
        }

        [HttpGet]
        public JsonResult SignatureProcess(long formId, string checkboxId, bool isCheckboxChecked)
        {
            // TODO: add user checking
            //       add formId checking
            //       add signatureCheckboxId / isSignatureCheckboxChecked checking

            #region Determine which properties were affected. Getting affected PropertyLinker
            foreach (PropertyTypes type in Enum.GetValues(typeof(PropertyTypes)).Cast<PropertyTypes>())
            {
                IPropertyLinker propertyLinker = PropertyLinkerFactory.CreatePropertyLinker(type);
                if (PropertyLinkerHandler.IsPropertyLinkerAffected(propertyLinker, checkboxId))
                {
                    break;
                }
            }

            if(PropertyLinkerHandler.AffectedPropertyLinker == null)
            {
                JsonResult errorResponse = new JsonResult(new
                {
                    status = "error",
                    message = $"{DateTime.Now}: Signature process is not possible." +
                              $"Neither objectives nor results are involved into signature process.",
                });
                return errorResponse;
            }
            #endregion

            #region Get property-value pairs which should be saved in Database
            Dictionary<string, object> propertiesValues =
                PropertyLinkerHandler.GetPropertiesValues(checkboxId, isCheckboxChecked);

            if(propertiesValues.Count == 0)
            {
                JsonResult errorResponse = new JsonResult(new
                {
                    status = "error",
                    message = $"{DateTime.Now}: Signature process is not possible." +
                              $"No signature data are affected.",
                });
                return errorResponse;
            }
            #endregion

            #region Get form from database and check signature possibility
            Form form = formRepository.GetFormSignatureData(formId);
            if(PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyTypes.Objectives &&
               !FormDataHandler.IsObjectivesSignaturePossible(form))
            {
                JsonResult errorResponse = new JsonResult(new
                {
                    status = "error",
                    message = $"{DateTime.Now}: Signature process is not possible. Objectives should be freezed at first.",
                });
                return errorResponse;
            }

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyTypes.Results &&
               !FormDataHandler.IsResultsSignaturePossible(form))
            {
                JsonResult errorResponse = new JsonResult(new
                {
                    status = "error",
                    message = $"{DateTime.Now}: Signature process is not possible. Results should be freezed at first.",
                });
                return errorResponse;
            }
            #endregion

            #region Fill property-value pair with User signature and Update Form data
            FormDataHandler.PrepareSignatureData(ref propertiesValues);
            FormDataHandler.UpdateSignatureFormData(form, propertiesValues);
            FormDataHandler.UpdateLastSavedFormData(form);
            formRepository.UpdateForm(form);
            #endregion

            JsonResult response = new JsonResult(new
            {
                status = "success",
                message = $"{DateTime.Now}: Signature data were successfully updated.",
                propertiesValues = propertiesValues,
            });

            return response;
        }

        [HttpPost]
        public IActionResult SaveForm(ObjectivesDefinition objectivesDefinition,
                                      ObjectivesSignature objectivesSignature,
                                      ResultsDefinition resultsDefinition,
                                      ResultsSignature resultsSignature)
        {
            // TODO: check form stage:
            //       stage#1: only ObjectivesDefinition could be saved
            //       stage#2: only ObjectivesSignature could be saved
            //       stage#3: only ResultsDefinition could be saved
            //       stage#4: only ResultsSignature could be saved



            return RedirectToAction("Form");
        }
    }
}