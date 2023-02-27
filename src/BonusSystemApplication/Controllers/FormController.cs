using Microsoft.AspNetCore.Mvc;
using BonusSystemApplication.UserIdentiry;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.Models.Forms.Index;
using AutoMapper;
using BonusSystemApplication.Models.Forms.Edit;

//using Newtonsoft.Json.Serialization;

namespace BonusSystemApplication.Controllers
{
    public class FormController : Controller
    {
        private readonly ILogger<FormController> _logger;
        private IFormsService _formService;
        private readonly IMapper _mapper;

        public FormController(ILogger<FormController> logger,
                              IFormsService formService,
                              IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _formService = formService;
            UserData.UserId = 7;
        }

        public IActionResult Index(UserSelectionsDTO userSelections)
        {
            FormIndexViewModel formIndexViewModel = _mapper.Map<FormIndexViewModel>(_formService.GetFormIndexDTO(userSelections));
            return View(formIndexViewModel);
        }

        [HttpGet]
        public IActionResult Edit(long? Id)
        {
            #region Validate incoming form id
            if (Id == null || Id <= 0 ||
                !UserData.AvailableFormIds.Contains((long)Id))
            {
                return NotFound();
            }
            #endregion

            long formId = Convert.ToInt64(Id);
            FormDTO formDTO = _formService.GetFormDTO(formId);
            if (formDTO == null)
            {
                return NotFound();
            }

            FormEditViewModel formEditViewModel = new FormEditViewModel();
            formEditViewModel.Id = formDTO.Id;
            formEditViewModel.IsObjectivesFreezed = formDTO.IsObjectivesFreezed;
            formEditViewModel.IsResultsFreezed = formDTO.IsResultsFreezed;
            formEditViewModel.Definition = _mapper.Map<DefinitionVM>(_formService.GetDefinitionDTO(formId));
            formEditViewModel.Conclusion = _mapper.Map<ConclusionVM>(_formService.GetConclusionDTO(formId));
            formEditViewModel.Signatures = _mapper.Map<SignaturesVM>(_formService.GetSignaturesDTO(formId));
            formEditViewModel.ObjectivesResults = _mapper.Map<IList<ObjectiveResultVM>>(_formService.GetObjectivesResultsDTO(formId));
            formEditViewModel.WorkprojectSelectList = _formService.GetWorkprojectsNames();
            formEditViewModel.EmployeeSelectList = _formService.GetUsersNames();
            formEditViewModel.PeriodSelectList = _formService.GetPeriodsNames();

            return View(formEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(FormEditViewModel formEditViewModel)
        {
            #region Validate ViewModel
            if (formEditViewModel.Id < 0 || !UserData.AvailableFormIds.Contains(formEditViewModel.Id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                formEditViewModel.WorkprojectSelectList = _formService.GetWorkprojectsNames();
                formEditViewModel.EmployeeSelectList = _formService.GetUsersNames();
                formEditViewModel.PeriodSelectList = _formService.GetPeriodsNames();
                return View(formEditViewModel);
            }
            #endregion

            // TODO: provide Update process
            //       for any Id same rules, except one (applied in Update method in repository):
            //       id == 0: create new Form and put it in DB
            //       id != 0: load formId from DB and update it

            // TODO: check what could be saved: formViewModel.IsStates and Signatures (see SaveProcess())
            //       launch Update (formViewModel, PartsToSave) method:
            //       if Id == 0: provide checks for: Definition, Objectives, Results (recalculate based on Objectives of this formViewModel)
            //       if id != 0: get Form with Id,
            //                   provide checks for: Definition, Objectives, Results (recalculate based on Objectives of loaded Form data)

            #region Determine parts allowable to update
            FormDTO formDTO = _formService.UpdateForm(_mapper.Map<DefinitionDTO>(formEditViewModel.Definition),
                                                      _mapper.Map<ConclusionDTO>(formEditViewModel.Conclusion),
                                                      _mapper.Map<SignaturesDTO>(formEditViewModel.Signatures),
                                                      _mapper.Map<IList<ObjectiveResultDTO>>(formEditViewModel.ObjectivesResults));
            #endregion
            return View(formEditViewModel);
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
        public IActionResult PromoteSelectedForms(List<long> selectedFormIds)
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

            return new JsonResult(new
            {
                status = "success",
                message = "Operation was complited successfully",
                workprojectDescription = _formService.GetWorkprojectDescription(workprojectId),
            });
        }

        /// <summary>
        /// Action for Ajax request method requestEmployeeData
        /// </summary>
        /// <param name="employeeId">selected employee id</param>
        /// <returns>status, message, teamName, positionName and pid of employee</returns>
        public JsonResult GetEmployeeData(long employeeId)
        {
            #region check requested id
            if (employeeId <= 0)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad id was requested"
                });
            }
            #endregion
            
            EmployeeDTO employeeDTO = _formService.GetEmployeeDTO(employeeId);

            return new JsonResult(new
            {
                status = "success",
                message = "Operation was complited successfully",
                employeeTeam = employeeDTO.TeamName,
                employeePosition = employeeDTO.PositionName,
                employeePid = employeeDTO.Pid,
            });
        }

        public void ChangeState(long formId)
        {
            // TODO: get current formId and user from Session
            //       check that user has permission to modify form

            // form should be saved

            // to find form by id
            // extract all states and all signatures into new Form object
            // invert state and remove corrsponding signatues
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
            Form statesAndSignatures = formRepository.GetIsFreezedAndSignatures(formId);
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
        public IActionResult SaveProcess(HomeFormViewModel formViewModel)
        {

            long formId = formViewModel.Id;

            // TODO: add user checking
            //       add formId checking

            if (formId == 0)
            {
                // TODO: save new Form
                //       return to client
            }
            else
            {
            }

            #region Getting Form IsFreezed states and all Signatures
            Form statesAndSignatures = formRepository.GetIsFreezedAndSignatures(formId);
            #endregion

            if (!ModelState.IsValid)
            {
                // the model was not valid => redisplay the form so that 
                // the user can fix errors

                return View("Form", formViewModel);
            }

            // TODO: Use ModelState to check model binding status

            SaveConfigurator saveConfigurator = new SaveConfigurator(statesAndSignatures);
            //if(!saveConfigurator.IsDataCouldBeUpdated(formRepository,
            //                                          definition,
            //                                          objectivesResults,
            //                                          conclusion))
            //{
            //    // TODO: update is not possible
            //    return RedirectToAction("Form");
            //}

            return RedirectToAction("Form");
        }
    }
}