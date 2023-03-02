using Microsoft.AspNetCore.Mvc;
using BonusSystemApplication.UserIdentiry;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.Models.Forms.Index;
using AutoMapper;
using BonusSystemApplication.Models.Forms.Edit;
using BonusSystemApplication.BLL.Infrastructure;

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
            if (Id == null || Id < 0 ||
                !UserData.AvailableFormIds.Contains((long)Id))
            {
                return NotFound();
            }
            #endregion

            long formId = Convert.ToInt64(Id);
            FormEditViewModel formEditViewModel = new FormEditViewModel();

            if (formId == 0)
            {
                formEditViewModel.Id = 0;
                formEditViewModel.IsResultsFreezed = false;
                formEditViewModel.IsObjectivesFreezed = false;
                formEditViewModel.Definition = new DefinitionVM();
                formEditViewModel.Conclusion = new ConclusionVM();
                formEditViewModel.Signatures = new SignaturesVM();

                List<ObjectiveResultVM> objectivesResults = new List<ObjectiveResultVM>();
                for (int i = 0; i < 10; i++)
                {
                    ObjectiveResultVM objectiveResult = new ObjectiveResultVM()
                    {
                        Row = i + 1,
                        Objective = new ObjectiveVM(),
                        Result = new ResultVM(),
                    };
                    objectivesResults.Add(objectiveResult);
                }
                formEditViewModel.ObjectivesResults = objectivesResults;
            }
            else
            {
                FormDTO formDTO = _formService.GetFormDTO(formId);
                if (formDTO == null)
                {
                    return NotFound();
                }

                formEditViewModel.Id = formDTO.Id;
                formEditViewModel.IsObjectivesFreezed = formDTO.IsObjectivesFreezed;
                formEditViewModel.IsResultsFreezed = formDTO.IsResultsFreezed;
                formEditViewModel.Definition = _mapper.Map<DefinitionVM>(_formService.GetDefinitionDTO(formId));
                formEditViewModel.Conclusion = _mapper.Map<ConclusionVM>(_formService.GetConclusionDTO(formId));
                formEditViewModel.Signatures = _mapper.Map<SignaturesVM>(_formService.GetSignaturesDTO(formId));
                formEditViewModel.ObjectivesResults = _mapper.Map<IList<ObjectiveResultVM>>(_formService.GetObjectivesResultsDTO(formId));
            }

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
            return View("Edit");
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

        /// <summary>
        /// Action for Ajax request on change event for id='js-signature'
        /// </summary>
        /// <param name="formId">id of loaded form</param>
        /// <param name="checkboxId">id of checked checkbox</param>
        /// <param name="isCheckboxChecked">checkbox current status</param>
        /// <returns></returns>
        public JsonResult SignatureProcess(long formId, string checkboxId, bool isCheckboxChecked)
        {
            // TODO: add user checking
            //       add formId checking

            try
            {
                Dictionary<string, object> propertiesValues = _formService.UpdateAndReturnSignatures(formId,
                                                                                                     checkboxId,
                                                                                                     isCheckboxChecked);
                return new JsonResult(new
                {
                    status = "success",
                    message = $"Signature data were successfully updated.",
                    propertiesValues = propertiesValues,
                });
            }
            catch (ValidationException ex)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = ex.Message,
                });
            }
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

    }
}