using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BonusSystemApplication.Handlers;
using BonusSystemApplication.Models.Forms.Index;
using BonusSystemApplication.Models.Forms.Edit;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.BLL.UserIdentiry;

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

        public async Task<IActionResult> Index(UserSelectionsVM userSelectionsVM)
        {
            FormIndexViewModel formIndexViewModel = new FormIndexViewModel();
            try
            {
                UserSelectionsDTO userSelectionsDTO = _mapper.Map<UserSelectionsDTO>(userSelectionsVM);
                formIndexViewModel = _mapper.Map<FormIndexViewModel>(await _formService.GetFormIndexDtoAsync(userSelectionsDTO));

                try
                {
                    string[] errors = (string[])TempData["errorMessages"]!;
                    if (errors != null)
                        formIndexViewModel.ErrorMessages = errors.ToList();
                }
                catch (Exception ex) when (ex is InvalidCastException)
                {
                    _logger.LogError($"Message: {ex.Message}.\n" +
                                     $"StackTrace: {ex.StackTrace}.\n" +
                                     $"TargetSite = {ex.TargetSite}.\n");
                    formIndexViewModel.ErrorMessages.Add("An error occured during last operation. " +
                                                         "Try to restart application. If the problem persists, " +
                                                         "see your system administrator.");
                }
            }
            catch (ValidationException ex)
            {
                formIndexViewModel.ErrorMessages.Add(ex.Message);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                formIndexViewModel.ErrorMessages.Add("Unable to prepare available forms. " +
                                                     "Try again, and if the problem persists, " +
                                                     "see your system administrator.");
            }

            return View(formIndexViewModel);
        }

        public async Task<IActionResult> Edit(long? Id)
        {
            #region Validate incoming form id
            if (Id == null || Id < 0)
                return NotFound();

            if (Id > 0 && !UserData.AvailableFormIds.Contains((long)Id))
                return NotFound();
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
                    formEditViewModel = _mapper.Map<FormEditViewModel>(await _formService.GetFormDtoAsync(formId));
                }
                catch (ValidationException ex)
                {
                    TempData["errorMessages"] = ex.Message.ToList();
                    return RedirectToAction(nameof(Index));
                }
                catch (AutoMapperMappingException ex)
                {
                    _logger.LogError($"Message: {ex.Message}.\n" +
                                     $"StackTrace: {ex.StackTrace}.\n" +
                                     $"TargetSite = {ex.TargetSite}.\n");

                    TempData["errorMessages"] = "Unable to prepare selected form for editing. " +
                                                "Try again, and if the problem persists, " +
                                                "see your system administrator.".ToList();
                    return RedirectToAction(nameof(Index));
                }
            }

            SelectListsCreator selectListsCreator = new SelectListsCreator(_formService);
            await selectListsCreator.CreateSelectListsAsync(formEditViewModel);

            return View(formEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FormEditViewModel formEditViewModel)
        {
            try
            {
                long formId = formEditViewModel.Id;

                #region ModelState and formId validation
                if (formId < 0)
                    return NotFound();

                if (formId > 0 && !UserData.AvailableFormIds.Contains(formId))
                    return NotFound();

                SelectListsCreator selectListsCreator = new SelectListsCreator(_formService);
                await selectListsCreator.CreateSelectListsAsync(formEditViewModel);

                if (!ModelState.IsValid)
                {
                    return View(formEditViewModel);
                }
                #endregion

                DefinitionDTO definitionDTO = _mapper.Map<DefinitionDTO>(formEditViewModel.Definition);
                ConclusionDTO conclusionDTO = _mapper.Map<ConclusionDTO>(formEditViewModel.Conclusion);
                List<ObjectiveResultDTO> objectiveResultDTOs = _mapper.Map<List<ObjectiveResultDTO>>(formEditViewModel.ObjectivesResults);

                if (formId > 0)
                {
                    await _formService.UpdateFormAsync(formId,
                                                       definitionDTO,
                                                       conclusionDTO,
                                                       objectiveResultDTOs);

                    formEditViewModel.Signatures = _mapper.Map<SignaturesDTO, SignaturesVM>(await _formService.GetSignaturesDtoAsync(formId));

                    return RedirectToAction(nameof(Edit), new { Id = formId });
                }
                else // formId = 0
                {
                    await _formService.CreateFormAsync(definitionDTO,
                                                       conclusionDTO,
                                                       objectiveResultDTOs);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                ModelState.AddModelError("", "Unable to perform operation. " +
                                             "Try again, and if the problem persists, " +
                                             "see your system administrator.");
            }

            return View(nameof(Edit), formEditViewModel);
        }

        /// <summary>
        /// Changes IsFrozen state of edited Form
        /// </summary>
        /// <param name="formEditViewModel">edited Form</param>
        /// <param name="changeToState">frozen or unfrozen</param>
        /// <param name="objectivesOrResults">objectives or results</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangeState(FormEditViewModel formEditViewModel, string? changeToState, string? objectivesOrResults)
        {
            try
            {
                long formId = formEditViewModel.Id;

                #region ModelState and formId validation
                if (formId < 0)
                    return NotFound();

                if (formId > 0 && !UserData.AvailableFormIds.Contains(formId))
                    return NotFound();

                SelectListsCreator selectListsCreator = new SelectListsCreator(_formService);
                await selectListsCreator.CreateSelectListsAsync(formEditViewModel);

                formEditViewModel.Signatures = _mapper.Map<SignaturesDTO, SignaturesVM>(await _formService.GetSignaturesDtoAsync(formId));

                if (!ModelState.IsValid)
                {
                    return View(nameof(Edit), formEditViewModel);
                }
                #endregion

                DefinitionDTO definitionDTO = _mapper.Map<DefinitionDTO>(formEditViewModel.Definition);
                ConclusionDTO conclusionDTO = _mapper.Map<ConclusionDTO>(formEditViewModel.Conclusion);
                List<ObjectiveResultDTO> objectiveResultDTOs = _mapper.Map<List<ObjectiveResultDTO>>(formEditViewModel.ObjectivesResults);

                #region Change IsFrozen state
                if (changeToState != null && objectivesOrResults != null)
                {
                    if (formId == 0)
                        throw new ValidationException("Please save the form before changing state");

                    if (changeToState == "frozen")
                    {
                        await _formService.UpdateFormAsync(formId,
                                                           definitionDTO,
                                                           conclusionDTO,
                                                           objectiveResultDTOs);
                        await _formService.ChangeStateAsync(formId,
                                                            changeToState,
                                                            objectivesOrResults);
                    }
                    else if (changeToState == "unfrozen")
                    {
                        await _formService.ChangeStateAsync(formId,
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
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                ModelState.AddModelError("", "Unable to perform operation. " +
                                             "Try again, and if the problem persists, " +
                                             "see your system administrator.");
            }

            return View(nameof(Edit), formEditViewModel);
        }

        /// <summary>
        /// Opens in Edit view a copy of existing form
        /// as a prefilled new Form
        /// </summary>
        /// <param name="ids">selected form ids</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Form/New")]
        public async Task<IActionResult> OpenPrefilled(List<long> ids)
        {
            FormIdsValidator validator = new FormIdsValidator();
            validator.ValidateFormIds(ids);

            if (ids.Count() == 0)
                return RedirectToAction(nameof(Index));

            try
            {
                // only first selected formId will be operated
                FormEditViewModel formEditViewModel = _mapper.Map<FormEditViewModel>(await _formService.GetPrefilledFormDtoAsync(ids[0]));

                SelectListsCreator selectListsCreator = new SelectListsCreator(_formService);
                await selectListsCreator.CreateSelectListsAsync(formEditViewModel);

                return View(nameof(Edit), formEditViewModel);
            }
            catch (ValidationException ex)
            {
                TempData["errorMessages"] = new List<string> { ex.Message };
                return RedirectToAction(nameof(Index));
            }
            catch (AutoMapperMappingException ex)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                TempData["errorMessages"] = new List<string>
                {
                    "Unable to prepare selected form for editing. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator."
                };
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Creates a copy of selected form in the Database,
        /// and changes its Period to the next one
        /// </summary>
        /// <param name="ids">selected form ids</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Promote(List<long> ids)
        {
            try
            {
                FormIdsValidator validator = new FormIdsValidator();
                validator.ValidateFormIds(ids);

                if (ids.Count() == 0)
                    return RedirectToAction(nameof(Index));

                List<string> errorMessages = new List<string>();
                foreach (long formId in ids)
                {
                    string errorMessage = await _formService.PromoteFormAsync(formId);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessages.Add(errorMessage);
                        errorMessage = string.Empty;
                    }
                }

                TempData["errorMessages"] = errorMessages;
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                TempData["errorMessages"] = new List<string> { ex.Message };
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Deletes forms with selected ids from database
        /// </summary>
        /// <param name="ids">selected form ids</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(List<long> ids)
        {
            // !!! IMPORTANT !!!
            // is in progress: only for demostaiting of functionality
            // in according to Business rules using of this method should be restricted
            // only Head of HR should be able to see corresponding button and launch the process

            FormIdsValidator validator = new FormIdsValidator();
            validator.ValidateFormIds(ids);

            if (ids.Count() == 0)
            return RedirectToAction("Index");

            try
            {
                List<string> errorMessages = new List<string>();
                foreach (long formId in ids)
                {
                    string message = await _formService.DeleteFormAsync(formId);
                    if (!string.IsNullOrEmpty(message))
                    {
                        errorMessages.Add(message);
                        message = string.Empty;
                    }
                    TempData["errorMessages"] = errorMessages;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                TempData["errorMessages"] = new List<string> { ex.Message };
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Requests workproject description from Database.
        /// Action for Ajax request method requestWorkprojectDescription
        /// </summary>
        /// <param name="workprojectId">selected workproject id</param>
        /// <returns>status, message and workprojectDescription</returns>
        public async Task<JsonResult> GetWorkprojectDescription(long workprojectId)
        {
            #region check requested id
            if(workprojectId <= 0)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad request."
                });
            }
            #endregion
            try
            {
                return new JsonResult(new
                {
                    status = "success",
                    message = "Operation was complited successfully",
                    workprojectDescription = await _formService.GetWorkprojectDescriptionAsync(workprojectId),
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

        /// <summary>
        /// Requests employee data from Database.
        /// Action for Ajax request method requestEmployeeData
        /// </summary>
        /// <param name="employeeId">selected employee id</param>
        /// <returns>status, message, teamName, positionName and pid of employee</returns>
        public async Task<JsonResult> GetEmployeeData(long employeeId)
        {
            #region check requested id
            if (employeeId <= 0)
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Bad request."
                });
            }
            #endregion
            try
            {
                EmployeeDTO employeeDTO = await _formService.GetEmployeeDtoAsync(employeeId);

                return new JsonResult(new
                {
                    status = "success",
                    message = "Operation was complited successfully",
                    employeeTeam = employeeDTO.TeamName,
                    employeePosition = employeeDTO.PositionName,
                    employeePid = employeeDTO.Pid,
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

        /// <summary>
        /// Provides signature process.
        /// Action for Ajax request on change event for id='js-signature'
        /// </summary>
        /// <param name="formId">id of loaded form</param>
        /// <param name="checkboxId">id of checked checkbox</param>
        /// <param name="isCheckboxChecked">checkbox current status</param>
        /// <returns></returns>
        public async Task<JsonResult> SignatureProcess(long formId, string checkboxId, bool isCheckboxChecked)
        {
            if (formId <= 0 || !UserData.AvailableFormIds.Contains(formId))
            {
                return new JsonResult(new
                {
                    status = "error",
                    message = "Unable to perform signature operation."
                });
            }

            try
            {
                Dictionary<string, object> propertiesValues = await _formService.UpdateAndReturnSignaturesAsync(formId,
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
    }
}