using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.Processes;
using BonusSystemApplication.BLL.Processes.Signing;
using BonusSystemApplication.BLL.Processes.Filtering;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.UserIdentiry;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.BLL.Services
{
    public class FormsService : IFormsService
    {
        private readonly ILogger<FormsService> _logger;
        private readonly IMapper _mapper;

        private IFormRepository _formRepository;
        private IUserRepository _userRepository;
        private IWorkprojectRepository _workprojectRepository;
        private IGlobalAccessRepository _globalAccessRepository;

        private IDefinitionRepository _definitionRepository;
        private IConclusionRepository _conclusionRepository;
        private ISignaturesRepository _signaturesRepository;
        private IObjectiveResultRepository _objectiveResultRepository;


        public FormsService(ILogger<FormsService> logger,
                            IMapper mapper,
                            IFormRepository formRepo,
                            IUserRepository userRepo,
                            IDefinitionRepository definitionRepo,
                            IConclusionRepository conclusionRepo,
                            ISignaturesRepository signaturesRepo,
                            IWorkprojectRepository workprojectRepo,
                            IGlobalAccessRepository globalAccessRepo,
                            IObjectiveResultRepository objectiveResultRepo)
        {
            _logger = logger;
            _mapper = mapper;

            _formRepository = formRepo;
            _userRepository = userRepo;
            _workprojectRepository = workprojectRepo;
            _globalAccessRepository = globalAccessRepo;

            _definitionRepository = definitionRepo;
            _conclusionRepository = conclusionRepo;
            _signaturesRepository = signaturesRepo;
            _objectiveResultRepository = objectiveResultRepo;
        }

        public FormIndexDTO GetFormIndexDTO(UserSelectionsDTO userSelections)
        {
            #region Getting global accesses user has
            IEnumerable<GlobalAccess> globalAccesses = _globalAccessRepository.GetGlobalAccessesByUserId(UserData.UserId);
            #endregion

            #region Getting form Ids available for User
            //Form Ids where user has Global accesses
            IEnumerable<long> formIdsWithGlobalAccess = _definitionRepository.GetFormIdsWhereGlobalAccess(globalAccesses);

            //Form Ids where user has Local access
            // TODO: replace _formRespository to _localAccessRepository
            IEnumerable<long> formIdsWithLocalAccess = _formRepository.GetFormIdsWhereLocalAccess(UserData.UserId);

            //Form Ids where user has any Participation
            IEnumerable<long> formIdsWithParticipation = _definitionRepository.GetFormIdsWhereParticipation(UserData.UserId);

            //Form Ids unioning and sorting
            List<long> availableFormIds = formIdsWithParticipation.Union(formIdsWithLocalAccess)
                                                                  .Union(formIdsWithGlobalAccess)
                                                                  .OrderBy(id => id)
                                                                  .ToList();
            #endregion

            #region Load data from database into forms and get corresponding permissions
            FormDataAvailable formDataAvailable = new FormDataAvailable(
                           _formRepository.GetForms(availableFormIds)
                          .ToDictionary(form => form,
                                        form => FormDataExtractor.ExtractPermissions(form,
                                                                                     UserData.UserId,
                                                                                     formIdsWithGlobalAccess,
                                                                                     formIdsWithLocalAccess,
                                                                                     formIdsWithParticipation)));
            #endregion

            UserData.AvailableFormIds = availableFormIds;

            // TODO: to analyse: this time there are Several big classes for data preparing
            //       FormDataAvailable, FormDataSorted
            //       and it is necessary to manage same information in similar manner several times
            //       Perhaps it will be useful to create a class for each information/type
            //       and operate it inside this class. If a new information will appear
            //       it will be necessary to create new object and attach it to viewmodel...

            // TODO: FormDataSorted: Sorted<Data> collections are not used. FormDataSorted could be removed.

            // TODO: userSelections is DTO => exclude any methods into separate class

            userSelections.PrepareSelections(formDataAvailable);
            FormDataSorted formDataSorted = new FormDataSorted(formDataAvailable, userSelections);

            #region Prepare TableRows: table's content
            List<TableRowDTO> tableRows = formDataSorted.SortedFormPermissions
                .Select(pair => new TableRowDTO
                {
                    Id = pair.Key.Id,
                    EmployeeFullName = $"{pair.Key.Definition.Employee.LastNameEng} {pair.Key.Definition.Employee.FirstNameEng}",
                    WorkprojectName = pair.Key.Definition.Workproject?.Name,
                    DepartmentName = pair.Key.Definition.Employee.Department?.Name,
                    TeamName = pair.Key.Definition.Employee.Team?.Name,
                    LastSavedAt = pair.Key.LastSavedAt,
                    Period = pair.Key.Definition.Period.ToString(),
                    Year = pair.Key.Definition.Year.ToString(),
                    Permissions = pair.Value.Select(p => p.ToString()).ToList(),
                })
                .ToList();
            #endregion

            #region Prepare SelectLists: dropdown's content
            SelectListsDTO selectLists = new SelectListsDTO(formDataAvailable, userSelections);
            #endregion

            return new FormIndexDTO
            {
                TableRows = tableRows,
                SelectLists = selectLists,
            };
        }
        public FormDTO GetFormDTO(long formId)
        {
            FormDTO formDTO = new FormDTO();
            try
            {
                formDTO = _mapper.Map<FormDTO>(_formRepository.GetForm(formId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: {nameof(GetFormDTO)}. Params: {nameof(formId)} = {formId} " +
                                 $"EF msg: {ex.Message}", "");
                throw new ValidationException("Unable to get form data. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }

            PrepareFormDTOForPresentation(formDTO);
            return formDTO;
        }
        public FormDTO GetIsFrozenStates(long formId)
        {
            FormDTO formDTO = new FormDTO();
            try
            {
                formDTO = _mapper.Map<FormDTO>(_formRepository.GetStates(formId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: {nameof(GetIsFrozenStates)}. Params: {nameof(formId)} = {formId} " +
                                 $"EF msg: {ex.Message}", "");
                throw new ValidationException("Unable to get form data. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }

            return formDTO;
        }

        public StatesAndSignaturesDTO GetStatesAndSignaturesDTO(long formId)
        {
            Form statesAndSignatures = _formRepository.GetStatesAndSignatures(formId);
            return new StatesAndSignaturesDTO
            {
                AreObjectivesFrozen = statesAndSignatures.AreObjectivesFrozen,
                AreResultsFrozen = statesAndSignatures.AreResultsFrozen,
                SignaturesDTO = _mapper.Map<Signatures, SignaturesDTO>(statesAndSignatures.Signatures),
            };
        }

        public DefinitionDTO GetDefinitionDTO(long defintionId)
        {
            return _mapper.Map<DefinitionDTO>(_definitionRepository.GetDefinitionFull(defintionId));
        }
        public ConclusionDTO GetConclusionDTO(long conclusionId)
        {
            return _mapper.Map<ConclusionDTO>(_conclusionRepository.GetConclusion(conclusionId));
        }
        public SignaturesDTO GetSignaturesDTO(long signaturesId)
        {
            return _mapper.Map<SignaturesDTO>(_signaturesRepository.GetSignatures(signaturesId));
        }
        public IList<ObjectiveResultDTO> GetObjectivesResultsDTO(long formId)
        {
            return _mapper.Map<IList<ObjectiveResultDTO>>(_objectiveResultRepository.GetObjectivesResults(formId));
        }


        public Dictionary<string, string> GetWorkprojectIdsNames()
        {

            return _workprojectRepository.GetWorkprojectsNames()
                                  .OrderBy(wp => wp.Id)
                                  .ToDictionary(w => w.Id.ToString(),
                                                w => w.Name);
        }
        public Dictionary<string, string> GetUserIdsNames()
        {
            return _userRepository.GetUsersNames()
                                  .OrderBy(u => u.LastNameEng)
                                  .ToDictionary(u => u.Id.ToString(),
                                                u => $"{u.LastNameEng} {u.FirstNameEng}");
        }
        public List<string> GetPeriodNames()
        {
            return Enum.GetNames(typeof(Periods)).ToList();
        }


        public string GetWorkprojectDescription(long workprojectId)
        {
            string? description = _workprojectRepository.GetWorkprojectData(workprojectId).Description;
            return description == null ? string.Empty : description;
        }
        public EmployeeDTO GetEmployeeDTO(long userId)
        {
            User userData = _userRepository.GetUserData(userId);
            return new EmployeeDTO
            {
                TeamName = userData.Team?.Name == null ? string.Empty: userData.Team.Name,
                PositionName = userData.Position?.NameEng == null ? string.Empty : userData.Position.NameEng,
                DepartmentName = userData.Department?.Name == null ? string.Empty : userData.Department.Name,
                Pid = userData.Pid,
            };
        }

        public Dictionary<string, object> UpdateAndReturnSignatures(long formId,
                                                                    string checkboxId,
                                                                    bool isCheckboxChecked)
        {
            if (string.IsNullOrEmpty(checkboxId))
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Signature data is not affected.");
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

            if (PropertyLinkerHandler.AffectedPropertyLinker == null)
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Neither objectives nor results are involved into signature process.");
            }
            #endregion

            #region Get property-value pairs which should be saved in Database
            Dictionary<string, object> propertiesValues =
                PropertyLinkerHandler.GetPropertiesValues(checkboxId, isCheckboxChecked);

            if (propertiesValues.Count == 0)
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Signature data is not affected.");
            }
            #endregion

            #region Get form from database and check signature possibility
            Form statesAndSignatures = _formRepository.GetStatesAndSignatures(formId);

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForObjectives &&
               !FormDataHandler.IsObjectivesSignaturePossible(statesAndSignatures))
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Objectives should be frozen at first.");
            }

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForResults &&
               !FormDataHandler.IsResultsSignaturePossible(statesAndSignatures))
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Results should be frozen at first.");
            }
            #endregion

            #region Fill property-value pair with User signature and Update Form data
            FormDataHandler.PutUserSignature(ref propertiesValues);
            FormDataHandler.UpdateSignatures(statesAndSignatures, propertiesValues);
            FormDataHandler.UpdateLastSavedFormData(statesAndSignatures);

            try
            {
                _formRepository.UpdateSignatures(statesAndSignatures);
            }
            catch (ValidationException ex)
            {
                throw;
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to save changes. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
            #endregion

            return propertiesValues;
        }

        public void UpdateForm(long formId,
                               DefinitionDTO definitionDTO,
                               ConclusionDTO conclusionDTO,
                               List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            if (formId <= 0)
                throw new ValidationException($"Unable to perform operation. Unknown form.");

            try
            {
                Form statesAndSignatures = _formRepository.GetStatesAndSignatures(formId);

                if (statesAndSignatures.Signatures.AreResultsSigned)
                    throw new ValidationException("Unable to perform operation. Results are already signed.");

                if (statesAndSignatures.AreResultsFrozen)
                {
                    #region Conclusion's comments could be updated
                    _formRepository.UpdateConclusionComments(new Form
                    {
                        Id = formId,
                        LastSavedBy = UserData.GetUserName(),
                        LastSavedAt = DateTime.Now,
                        Conclusion = new Conclusion
                        {
                            ManagerComment = conclusionDTO.ManagerComment,
                            EmployeeComment = conclusionDTO.EmployeeComment,
                            OtherComment = conclusionDTO.OtherComment,
                        }
                    });
                    #endregion
                }
                else if (statesAndSignatures.Signatures.AreObjectivesSigned ||
                         statesAndSignatures.AreObjectivesFrozen)
                {
                    #region Results & Conclusion could be updated
                    List<ObjectiveResult> objectiveResults = (List<ObjectiveResult>)_objectiveResultRepository.GetObjectivesResults(formId);
                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.HandleResultsUpdateProcess(objectiveResults, objectiveResultDTOs);

                    ConclusionHandler conclusionHandler = new ConclusionHandler(conclusionDTO, objectiveResultDTOs);
                    conclusionHandler.HandleUpdateProcess();

                    _formRepository.UpdateResultsConclusion(new Form
                    {
                        Id = formId,
                        LastSavedAt = DateTime.Now,
                        LastSavedBy = UserData.GetUserName(),
                        Conclusion = _mapper.Map<Conclusion>(conclusionDTO),
                        ObjectivesResults = _mapper.Map<List<ObjectiveResult>>(objectiveResultDTOs),
                    });
                    #endregion
                }
                else
                {
                    #region Definition & Objectives & Results & Conclusion could be updated
                    DefinitionHandler definitionHandler = new DefinitionHandler(formId,
                                                                                _userRepository,
                                                                                _definitionRepository,
                                                                                _workprojectRepository);
                    definitionHandler.HandleUpdateProcess(definitionDTO);

                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.HandleObjectivesUpdateProcess(objectiveResultDTOs);

                    ConclusionHandler conclusionHandler = new ConclusionHandler(conclusionDTO, objectiveResultDTOs);
                    conclusionHandler.HandleUpdateProcess();

                    _formRepository.UpdateDefinitionObjectivesResultsConclusion(new Form
                    {
                        Id = formId,
                        LastSavedAt = DateTime.Now,
                        LastSavedBy = UserData.GetUserName(),

                        Definition = _mapper.Map<Definition>(definitionDTO),
                        Conclusion = _mapper.Map<Conclusion>(conclusionDTO),
                        ObjectivesResults = _mapper.Map<List<ObjectiveResultDTO>, List<ObjectiveResult>>(objectiveResultDTOs),
                    });
                    #endregion
                }
            }
            catch (ValidationException ex)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to perform operation. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }

        public void CreateForm(DefinitionDTO definitionDTO,
                               ConclusionDTO conclusionDTO,
                               List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            try
            {
                long formId = 0;
                DefinitionHandler definitionHandler = new DefinitionHandler(formId,
                                                                            _userRepository,
                                                                            _definitionRepository,
                                                                            _workprojectRepository);
                definitionHandler.HandleUpdateProcess(definitionDTO);

                ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                objectivesResultsHandler.HandleObjectivesUpdateProcess(objectiveResultDTOs);

                ConclusionHandler conclusionHandler = new ConclusionHandler(conclusionDTO, objectiveResultDTOs);
                conclusionHandler.HandleUpdateProcess();

                _formRepository.CreateForm(new Form
                {
                    Id = 0,
                    Signatures = new Signatures(),
                    LastSavedAt = DateTime.Now,
                    LastSavedBy = UserData.GetUserName(),
                    Definition = _mapper.Map<Definition>(definitionDTO),
                    Conclusion = _mapper.Map<Conclusion>(conclusionDTO),
                    ObjectivesResults = _mapper.Map<List<ObjectiveResult>>(objectiveResultDTOs),
                });
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");
                throw;
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to create form. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }

        /// <summary>
        /// Changes IsFrozen states for Form and drops collected signatures as well
        /// </summary>
        /// <param name="formId">id of Form to operate</param>
        /// <param name="changeToState">frozen or unfrozen</param>
        /// <param name="objectivesOrResults">objectives or results</param>
        /// <exception cref="ValidationException"></exception>
        public void ChangeState(long formId,
                                string changeToState,
                                string objectivesOrResults)
        {
            try
            {
                Form statesAndSignatures = _formRepository.GetStatesAndSignatures(formId);

                #region Freezing Objectives
                if (changeToState == "frozen" && objectivesOrResults == "objectives")
                {
                    if (statesAndSignatures.AreObjectivesFrozen)
                    {
                        _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                           $"Params: " +
                                           $"{nameof(formId)} = {formId}, " +
                                           $"{nameof(changeToState)} = {changeToState}, " +
                                           $"{nameof(objectivesOrResults)} = {objectivesOrResults}. ",
                                           $"Unable to freeze Objectives. Objectives are already frozen. ", "");
                        return;
                    }
                    if (statesAndSignatures.AreResultsFrozen)
                    {
                        _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                           $"Params: " +
                                           $"{nameof(formId)} = {formId}, " +
                                           $"{nameof(changeToState)} = {changeToState}, " +
                                           $"{nameof(objectivesOrResults)} = {objectivesOrResults}. ",
                                           $"Unable to freeze Objectives. Results are already frozen. ", "");
                        return;
                    }

                    // check if Definition is ready to be frozen
                    Definition definition = _definitionRepository.GetDefinition(formId);
                    DefinitionHandler definitionValidator = new DefinitionHandler();
                    definitionValidator.HandleChangeStateProcess(definition);

                    // check if Objectives are ready to be frozen
                    List<ObjectiveResult> objectiveResults = (List<ObjectiveResult>)_objectiveResultRepository.GetObjectivesResults(formId);
                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.ValidateObjectivesChangeStateProcess(objectiveResults);

                    _formRepository.UpdateStates(new Form
                    {
                        Id = formId,
                        LastSavedAt = DateTime.Now,
                        LastSavedBy = UserData.GetUserName(),
                        AreObjectivesFrozen = true,
                        AreResultsFrozen = statesAndSignatures.AreResultsFrozen,
                    });
                    return;
                }
                #endregion

                #region Freezing Results
                if (changeToState == "frozen" && objectivesOrResults == "results")
                {
                    if (!statesAndSignatures.AreObjectivesFrozen)
                    {
                        _logger.LogWarning($"{nameof(FormsService)}. Wrong client side's call of Method: {nameof(ChangeState)}. " +
                                           $"Unable to freeze Results. Objectives are not frozen. " +
                                           $"{nameof(formId)} = {formId}, {nameof(changeToState)} = {changeToState}, " +
                                           $"{nameof(objectivesOrResults)} = {objectivesOrResults}", "");
                        return;
                    }
                    if (statesAndSignatures.AreResultsFrozen)
                    {
                        _logger.LogWarning($"{nameof(FormsService)}. Wrong client side's call of Method: {nameof(ChangeState)}. " +
                                           $"Unable to freeze Results. Results are already frozen. " +
                                           $"{nameof(formId)} = {formId}, {nameof(changeToState)} = {changeToState}, " +
                                           $"{nameof(objectivesOrResults)} = {objectivesOrResults}", "");
                        return;
                    }

                    if (!statesAndSignatures.Signatures.AreObjectivesSigned)
                    {
                        throw new ValidationException("Unable to freeze results. Objectives are not signed.");
                    }
                    
                    List<ObjectiveResult> objectiveResults = (List<ObjectiveResult>)_objectiveResultRepository.GetObjectivesResults(formId);
                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.ValidateResultsChangeStateProcess(objectiveResults);

                    _formRepository.UpdateStates(new Form
                    {
                        Id = formId,
                        LastSavedAt = DateTime.Now,
                        LastSavedBy = UserData.GetUserName(),
                        AreObjectivesFrozen = statesAndSignatures.AreObjectivesFrozen,
                        AreResultsFrozen = true,
                    });
                    return;
                }
                #endregion

                #region Unfreezing Objectives
                if (changeToState == "unfrozen" && objectivesOrResults == "objectives")
                {
                    if (!statesAndSignatures.AreObjectivesFrozen)
                    {
                        _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                           $"Params: " +
                                           $"{nameof(formId)} = {formId}, " +
                                           $"{nameof(changeToState)} = {changeToState}, " +
                                           $"{nameof(objectivesOrResults)} = {objectivesOrResults}. ",
                                           $"Unable to unfreeze Objectives. Objectives are not frozen. ", "");
                        return;
                    }

                    // Drop all signatures
                    _signaturesRepository.DropSignatures(formId);

                    _formRepository.UpdateStates(new Form
                    {
                        Id= formId,
                        LastSavedAt = DateTime.Now,
                        LastSavedBy = UserData.GetUserName(),
                        AreObjectivesFrozen = false,
                        AreResultsFrozen = false,
                    });
                    return;
                }
                #endregion

                #region Unfreezing Results
                if (changeToState == "unfrozen" && objectivesOrResults == "results")
                {
                    if (!statesAndSignatures.AreObjectivesFrozen)
                    {
                        _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                           $"Params: " +
                                           $"{nameof(formId)} = {formId}, " +
                                           $"{nameof(changeToState)} = {changeToState}, " +
                                           $"{nameof(objectivesOrResults)} = {objectivesOrResults}. " +
                                           $"Unable to unfreeze Results. Objectives are not frozen. ", "");
                        return;
                    }
                    if (!statesAndSignatures.AreResultsFrozen)
                    {
                        _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                           $"Params: " +
                                           $"{nameof(formId)} = {formId}, " +
                                           $"{nameof(changeToState)} = {changeToState}, " +
                                           $"{nameof(objectivesOrResults)} = {objectivesOrResults}. " +
                                           $"Unable to unfreeze Results. Results are not frozen. ", "");
                        return;
                    }

                    // Drop signatures for Results only
                    _signaturesRepository.DropSignaturesForResults(formId);

                    _formRepository.UpdateStates(new Form
                    {
                        Id = formId,
                        LastSavedAt = DateTime.Now,
                        LastSavedBy = UserData.GetUserName(),
                        AreObjectivesFrozen = statesAndSignatures.AreObjectivesFrozen,
                        AreResultsFrozen = false,
                    });
                    return;
                }
                #endregion

                _logger.LogWarning($"Method: {nameof(ChangeState)}. Unknown call. " +
                                   $"Params: {nameof(changeToState)} = {changeToState}, " +
                                   $"{nameof(objectivesOrResults)} = {objectivesOrResults}.");
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");
                throw;
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is InvalidOperationException ||
                                       ex is DbUpdateException)
            {
                _logger.LogError($"Msg: {ex.Message}.\n" +
                                 $"IMsg: {ex.InnerException?.Message}\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to change state. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }


        private void PrepareFormDTOForPresentation(FormDTO formDTO)
        {
            #region Prepare Objectives and Results
            for (int i = 0; i < formDTO.ObjectivesResults.Count; i++)
            {
                if (!formDTO.ObjectivesResults[i].Objective.IsMeasurable)
                {
                    if (formDTO.ObjectivesResults[i].Objective.Target == null)
                        formDTO.ObjectivesResults[i].Objective.Target = "N/A";

                    if (formDTO.ObjectivesResults[i].Objective.Challenge == null)
                        formDTO.ObjectivesResults[i].Objective.Challenge = "N/A";

                    if (!formDTO.ObjectivesResults[i].Objective.IsKey)
                        if (formDTO.ObjectivesResults[i].Objective.Threshold == null)
                            formDTO.ObjectivesResults[i].Objective.Threshold = "N/A";
                }
            }
            #endregion region
        }
    }
}
