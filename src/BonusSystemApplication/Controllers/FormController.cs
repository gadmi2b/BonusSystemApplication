using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.Models.Forms.Index;
using BonusSystemApplication.Models.Forms.Edit;
using BonusSystemApplication.BLL.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        }

        public IActionResult Index(UserSelectionsDTO userSelections)
        {
            FormIndexViewModel formIndexViewModel = _mapper.Map<FormIndexViewModel>(_formService.GetFormIndexDTO(userSelections));
            return View(formIndexViewModel);
        }

        public IActionResult Edit(long? Id)
        {
            #region Validate incoming form id
            //if (Id == null || Id < 0 ||
            //    !UserData.AvailableFormIds.Contains((long)Id))
            //{
            //    return NotFound();
            //}
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
                try
                {
                    formEditViewModel = _mapper.Map<FormEditViewModel>(_formService.GetFormDTO(formId));
                }
                catch (ValidationException ex)
                {
                    // TODO: redirect to error page to show error message
                    return NotFound();
                }

                if (formEditViewModel == null)
                {
                    // TODO: redirect to error page to show error message
                    return NotFound();
                }
            }

            formEditViewModel.WorkprojectSelectList = _formService.GetWorkprojectsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.EmployeeSelectList = _formService.GetUsersNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.PeriodSelectList = _formService.GetPeriodsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();

            return View(formEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(FormEditViewModel formEditViewModel, string? act, string? type)
        {
            #region Validate ViewModel
            //if (formEditViewModel.Id < 0 || !UserData.AvailableFormIds.Contains(formEditViewModel.Id))
            //{
            //    return NotFound();
            //}

            formEditViewModel.WorkprojectSelectList = _formService.GetWorkprojectsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.EmployeeSelectList = _formService.GetUsersNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.PeriodSelectList = _formService.GetPeriodsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();

            if (!ModelState.IsValid)
            {
                //return View(formEditViewModel);
            }
            #endregion

            long formId = formEditViewModel.Id;
            DefinitionDTO definitionDTO = _mapper.Map<DefinitionDTO>(formEditViewModel.Definition);
            ConclusionDTO conclusionDTO = _mapper.Map<ConclusionDTO>(formEditViewModel.Conclusion);
            SignaturesDTO signaturesDTO = _mapper.Map<SignaturesDTO>(formEditViewModel.Signatures);
            IList<ObjectiveResultDTO> objectiveResultDTOs = _mapper.Map<IList<ObjectiveResultDTO>>(formEditViewModel.ObjectivesResults);

            if (formId == 0 && act != null && type != null)
            {
                // TODO: return message: "Please save the form before changing state"
                return View(formEditViewModel);
            }

            try
            {
                #region Create / Update form
                if (formId == 0)
                {
                    _formService.CreateForm(definitionDTO,
                                            objectiveResultDTOs);
                }
                else if (formId > 0)
                {
                    _formService.UpdateForm(formId,
                                            definitionDTO,
                                            conclusionDTO,
                                            signaturesDTO,
                                            objectiveResultDTOs);
                }
                #endregion

                #region Change IsFreezed state
                if (act != null && type != null)
                {
                    _formService.ChangeState(act!,
                                             type!,
                                             formId);

                    FormDTO formDTO = _formService.GetIsFreezedStates(formId);
                    formEditViewModel.IsObjectivesFreezed = formDTO.IsObjectivesFreezed;
                    formEditViewModel.IsResultsFreezed = formDTO.IsResultsFreezed;
                }
                #endregion
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(formEditViewModel);
        }
        
        [HttpPost]
        public IActionResult CreateFormBasedOnSelection(List<long> selectedFormIds)
        {
            #region Validation of selected form id
            //List<long> itemsToRemove = new List<long>();
            //foreach (long formId in selectedFormIds)
            //{
            //    if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
            //    {
            //        itemsToRemove.Add(formId);
            //    }
            //}
            //selectedFormIds.RemoveAll(x => itemsToRemove.Contains(x));
            //itemsToRemove.Clear();

            //if (selectedFormIds.Count() == 0)
            //{
            //    return RedirectToAction("Index");
            //}
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
            //List<long> itemsToRemove = new List<long>();
            //foreach (long formId in selectedFormIds)
            //{
            //    if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
            //    {
            //        itemsToRemove.Add(formId);
            //    }
            //}
            //selectedFormIds.RemoveAll(x => itemsToRemove.Contains(x));
            //itemsToRemove.Clear();

            //if (selectedFormIds.Count() == 0)
            //{
            //    return RedirectToAction("Index");
            //}
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
            //List<long> itemsToRemove = new List<long>();
            //foreach (long formId in selectedFormIds)
            //{
            //    if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
            //    {
            //        itemsToRemove.Add(formId);
            //    }
            //}
            //selectedFormIds.RemoveAll(x => itemsToRemove.Contains(x));
            //itemsToRemove.Clear();

            //if (selectedFormIds.Count() == 0)
            //{
            //    return RedirectToAction("Index");
            //}
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
                _logger.LogError(ex.Message);
                return new JsonResult(new
                {
                    status = "error",
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new JsonResult(new
                {
                    status = "error",
                    message = "An error occured during work of application. " +
                              "Please contact your system administrator.",
                });
            }
        }

    }
}