using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;
using Microsoft.Data.SqlClient.Server;
using BonusSystemApplication.Models.ViewModels;
using BonusSystemApplication.Models.ViewModels.Index;
using BonusSystemApplication.UserIdentiry;
using System.Text.Json;
using BonusSystemApplication.Models.BusinessLogic;
using BonusSystemApplication.Models.BusinessLogic.SignatureProcess;
using BonusSystemApplication.Models.BusinessLogic.SaveProcess;
using System.Security.Cryptography;

//using Newtonsoft.Json.Serialization;

namespace BonusSystemApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IGenericRepository<User> userGenRepository;
        private IGenericRepository<Workproject> workprojectGenRepository;

        private IGlobalAccessRepository globalAccessRepository;
        private IFormRepository formRepository;
        private IDefinitionRepository definitionRepository;
        private IWorkprojectRepository workprojectRepository;
        private IUserRepository userRepository;


        public HomeController(ILogger<HomeController> logger,
                              IGenericRepository<User> userGenRepo,
                              IGenericRepository<Workproject> workprojectGenRepo,
                              IGlobalAccessRepository globalAccessRepo,
                              IFormRepository formRepo,
                              IDefinitionRepository definitionRepo,
                              IWorkprojectRepository workprojectRepo,
                              IUserRepository userRepo)
        {
            _logger = logger;
            formRepository = formRepo;
            definitionRepository = definitionRepo;
            userGenRepository = userGenRepo;
            workprojectGenRepository = workprojectGenRepo;
            globalAccessRepository = globalAccessRepo;
            workprojectRepository = workprojectRepo;
            userRepository = userRepo;

            UserData.UserId = 7;
        }

        public IActionResult Index(UserSelections userSelections)
        {
            #region Global accesses for user
            IEnumerable<GlobalAccess> globalAccesses = globalAccessRepository.GetGlobalAccessesByUserId(UserData.UserId);
            #endregion

            #region Getting form Ids available for User
            //Form Ids where user has Global accesses
            IEnumerable<long> gAccessFormIds = definitionRepository.GetGlobalAccessFormIds(globalAccesses);

            //Form Ids where user has Local access
            IEnumerable<long> lAccessFormIds = formRepository.GetLocalAccessFormIds(UserData.UserId);

            //Form Ids where user has any Participation
            IEnumerable<long> participationFormIds = definitionRepository.GetParticipationFormIds(UserData.UserId);

            //Form Ids combination and sorting
            List<long> availableFormIds = gAccessFormIds
                                            .Union(lAccessFormIds)
                                            .Union(participationFormIds)
                                            .OrderBy(id => id)
                                            .ToList();
            #endregion

            #region Load data from database into forms and get corrsponding permissions
            FormDataAvailable formDataAvailable = new FormDataAvailable(
                                    formRepository.GetForms(availableFormIds)
                                    .ToDictionary(f => f,
                                                  f => FormDataExtractor.GetPermissions(f,
                                                                                        UserData.UserId,
                                                                                        gAccessFormIds,
                                                                                        lAccessFormIds,
                                                                                        participationFormIds)));

            #endregion

            UserData.AvailableFormIds = availableFormIds;

            // TODO: to analyse: this time there are Several big classes for data preparing
            //       FormDataAvailable, FormDataSorted
            //       and it is necessary to manage same information in similar manner several times
            //       Perhaps it will be useful to create a class for each information/type
            //       and operate it inside this class. If a new information will appear
            //       it will be necessary to create new object and attach it to viewmodel...

            // TODO: FormDataSorted: Sorted<Data> collections are not used. FormDataSorted could be removed.

            userSelections.PrepareSelections(formDataAvailable);
            FormDataSorted formDataSorted = new FormDataSorted(formDataAvailable, userSelections);

            #region Prepare TableRows: table content
            List<TableRow> tableRows = formDataSorted.SortedFormPermissions
                .Select(pair => new TableRow
                {
                    Id = pair.Key.Id,
                    EmployeeFullName = ($"{pair.Key.Definition.Employee.LastNameEng} {pair.Key.Definition.Employee.FirstNameEng}"),
                    WorkprojectName = pair.Key.Definition.Workproject?.Name,
                    DepartmentName = pair.Key.Definition.Employee.Department?.Name,
                    TeamName = pair.Key.Definition.Employee.Team?.Name,
                    LastSavedDateTime = pair.Key.LastSavedDateTime,
                    Period = pair.Key.Definition.Period.ToString(),
                    Year = pair.Key.Definition.Year.ToString(),
                    Permissions = pair.Value.Select(p => p.ToString()).ToList(),
                })
                .ToList();
            #endregion

            #region Prepare TableSelectLists: dropdowns content
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
                    Definition = new Definition(),
                    Conclusion = new Conclusion(),
                    Signatures = new Signatures
                    {
                        ForObjectives = new ForObjectives(),
                        ForResults = new ForResults(),
                    },
                };

                List<ObjectiveResult> objectivesResults = new List<ObjectiveResult>();
                for (int i = 0; i < 10; i++)
                {
                    ObjectiveResult objectiveResult = new ObjectiveResult()
                    {
                        Id = 0,
                        Row = i + 1,
                        Form = form,
                        Objective = new Objective(),
                        Result = new Result(),
                    };
                    objectivesResults.Add(objectiveResult);
                }
                form.ObjectivesResults = objectivesResults;
            }
            else
            {
                #region Validate selected form id
                if (id < 0 || !UserData.AvailableFormIds.Contains(id))
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

            // TODO: More thin ViewModel is required: Definition = form.Definition - is to dick
            //       I already took to form only necessary data
            //       So I need to send only this data to client (need mapper)

            #region Prepare HomeFormViewModel
            HomeFormViewModel homeFormViewModel = new HomeFormViewModel
            {
                Definition = form.Definition,
                ObjectivesResults = form.ObjectivesResults,
                Conclusion = form.Conclusion,
                Signatures = form.Signatures,

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

                TeamName = form.Definition.Employee?.Team?.Name,
                PositionName = form.Definition.Employee?.Position?.NameEng,
                Pid = form.Definition.Employee?.Pid,
                WorkprojectDescription = form.Definition.Workproject?.Description,
                IsObjectivesFreezed = form.IsObjectivesFreezed,
                IsResultsFreezed = form.IsResultsFreezed,
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
                if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
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
                if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
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
                if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
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

        public JsonResult SignatureProcess(long formId, string checkboxId, bool isCheckboxChecked)
        {
            // TODO: add user checking
            //       add formId checking

            if (string.IsNullOrEmpty(checkboxId))
            {
                JsonResult errorResponse = new JsonResult(new
                {
                    status = "error",
                    message = $"{DateTime.Now}: Signature process is not possible." +
                              $"Signature data is not affected.",
                });
                return errorResponse;
            }

            #region Determine which properties were affected. Getting affected PropertyLinker
            foreach (PropertyType type in Enum.GetValues(typeof(PropertyType)).Cast<PropertyType>())
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
                              $"Signature data is not affected.",
                });
                return errorResponse;
            }
            #endregion

            #region Get form from database and check signature possibility
            Form statesAndSignatures = formRepository.GetIsFreezedAndSignatureData(formId);
            if(PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForObjectives &&
               !FormDataHandler.IsObjectivesSignaturePossible(statesAndSignatures))
            {
                JsonResult errorResponse = new JsonResult(new
                {
                    status = "error",
                    message = $"{DateTime.Now}: Signature process is not possible. Objectives should be freezed at first.",
                });
                return errorResponse;
            }

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForResults &&
               !FormDataHandler.IsResultsSignaturePossible(statesAndSignatures))
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
            FormDataHandler.PutUserSignature(ref propertiesValues);
            FormDataHandler.UpdateSignatures(statesAndSignatures, propertiesValues);
            FormDataHandler.UpdateLastSavedFormData(statesAndSignatures);
            formRepository.UpdateFormSignatures(statesAndSignatures);
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
        public IActionResult SaveProcess(Definition definition,
                                         List<ObjectiveResult> objectivesResults,
                                         Conclusion conclusion)
        {
            // TODO: [BindNever] for Employee and Form is not working: ModelState is Invalid
            //       because these Properties are required
            //       Perhaps it will be better to make DefinitionView, ConclusionView etc
            //       or One ViewModel like HFVM but simplifyed till only necessary properties


            if (!ModelState.IsValid)
            {
                // the model was not valid => redisplay the form so that 
                // the user can fix errors
                return RedirectToAction("Form", new { id = definition.Id });
            }

            long formId = definition.Id;

            // TODO: add user checking
            //       add formId checking

            if(formId == 0)
            {
                // TODO: save new Form
                //       return to client
            }

            #region Getting Form IsFreezed states and all Signatures
            Form statesAndSignatures = formRepository.GetIsFreezedAndSignatureData(formId);
            #endregion

            // TODO: Use ModelState to check model binding status

            SaveConfigurator saveConfigurator = new SaveConfigurator(statesAndSignatures);
            if(!saveConfigurator.IsDataCouldBeUpdated(formRepository,
                                                      definition,
                                                      objectivesResults,
                                                      conclusion))
            {
                // TODO: update is not possible
                return RedirectToAction("Form");
            }

            return RedirectToAction("Form");
        }
    }
}