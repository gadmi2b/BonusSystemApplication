using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using BonusSystemApplication.Models.Forms.Index;
using BonusSystemApplication.Models.Forms.Edit;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.BLL.Services;

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
                formEditViewModel.AreResultsFrozen = false;
                formEditViewModel.AreObjectivesFrozen = false;
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

            formEditViewModel.WorkprojectSelectList = _formService.GetWorkprojectIdsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.EmployeeSelectList = _formService.GetUserIdsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.PeriodSelectList = _formService.GetPeriodNames()
                                        .Select(d => new SelectListItem { Value = d, Text = d })
                                        .ToList();
            return View(formEditViewModel);
        }

        [HttpPost]
        [Route("Form/Edit")]
        public IActionResult ChangeState(FormEditViewModel formEditViewModel, string? changeToState, string? objectivesOrResults)
        {
            long formId = formEditViewModel.Id;

            #region Validate ViewModel
            //if (formEditViewModel.Id < 0 || !UserData.AvailableFormIds.Contains(formEditViewModel.Id))
            //{
            //    return NotFound();
            //}

            formEditViewModel.WorkprojectSelectList = _formService.GetWorkprojectIdsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.EmployeeSelectList = _formService.GetUserIdsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.PeriodSelectList = _formService.GetPeriodNames()
                                        .Select(d => new SelectListItem { Value = d, Text = d })
                                        .ToList();
            formEditViewModel.Signatures = _mapper.Map<SignaturesDTO, SignaturesVM>(_formService.GetSignaturesDTO(formId));

            if (!ModelState.IsValid)
            {
                //return View(formEditViewModel);
            }
            #endregion

            DefinitionDTO definitionDTO = _mapper.Map<DefinitionDTO>(formEditViewModel.Definition);
            ConclusionDTO conclusionDTO = _mapper.Map<ConclusionDTO>(formEditViewModel.Conclusion);
            List<ObjectiveResultDTO> objectiveResultDTOs = _mapper.Map<List<ObjectiveResultDTO>>(formEditViewModel.ObjectivesResults);

            try
            {
                #region Change IsFrozen state
                if (changeToState != null && objectivesOrResults != null)
                {
                    if (formId == 0)
                    {
                        throw new ValidationException("Please save the form before changing state");
                    }

                    if (changeToState == "frozen")
                    {
                        _formService.UpdateForm(formId,
                                                definitionDTO,
                                                conclusionDTO,
                                                objectiveResultDTOs);
                        _formService.ChangeState(formId,
                                                 changeToState,
                                                 objectivesOrResults);
                    }
                    else if (changeToState == "unfrozen")
                    {
                        _formService.ChangeState(formId,
                                                 changeToState,
                                                 objectivesOrResults);
                    }
                }

                return RedirectToAction(nameof(Edit), new { Id = formId });
                #endregion
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }

            return View(nameof(Edit), formEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(FormEditViewModel formEditViewModel)
        {
            long formId = formEditViewModel.Id;

            #region Validate ViewModel
            //if (formEditViewModel.Id < 0 || !UserData.AvailableFormIds.Contains(formEditViewModel.Id))
            //{
            //    return NotFound();
            //}

            formEditViewModel.WorkprojectSelectList = _formService.GetWorkprojectIdsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.EmployeeSelectList = _formService.GetUserIdsNames()
                                        .Select(d => new SelectListItem { Value = d.Key, Text = d.Value })
                                        .ToList();
            formEditViewModel.PeriodSelectList = _formService.GetPeriodNames()
                                        .Select(d => new SelectListItem { Value = d, Text = d })
                                        .ToList();
            formEditViewModel.Signatures = _mapper.Map<SignaturesDTO, SignaturesVM>(_formService.GetSignaturesDTO(formId));

            if (!ModelState.IsValid)
            {
                //return View(formEditViewModel);
            }
            #endregion

            DefinitionDTO definitionDTO = _mapper.Map<DefinitionDTO>(formEditViewModel.Definition);
            ConclusionDTO conclusionDTO = _mapper.Map<ConclusionDTO>(formEditViewModel.Conclusion);
            List<ObjectiveResultDTO> objectiveResultDTOs = _mapper.Map<List<ObjectiveResultDTO>>(formEditViewModel.ObjectivesResults);

            try
            {
                if (formId > 0)
                {
                    _formService.UpdateForm(formId,
                                            definitionDTO,
                                            conclusionDTO,
                                            objectiveResultDTOs);
                    return RedirectToAction(nameof(Edit), new { Id = formId });
                }
                else
                {
                    _formService.CreateForm(definitionDTO,
                                            conclusionDTO,
                                            objectiveResultDTOs);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }

            return View(nameof(Edit), formEditViewModel);
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
                              "Please see your system administrator.",
                });
            }
        }
    }
}