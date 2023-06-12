using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.Processes;
using BonusSystemApplication.BLL.Processes.Signing;
using BonusSystemApplication.BLL.Processes.Filtering;
using BonusSystemApplication.BLL.Processes.Promoting;
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
        private ISignaturesRepository _signaturesRepository;
        private IObjectiveResultRepository _objectiveResultRepository;


        public FormsService(ILogger<FormsService> logger,
                            IMapper mapper,
                            IFormRepository formRepo,
                            IUserRepository userRepo,
                            IDefinitionRepository definitionRepo,
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
            _signaturesRepository = signaturesRepo;
            _objectiveResultRepository = objectiveResultRepo;
        }

        public FormIndexDTO GetFormIndexDTO(UserSelectionsDTO userSelections)
        {
            try
            {
                #region Getting global accesses user has
                IEnumerable<GlobalAccess> globalAccesses = _globalAccessRepository.GetGlobalAccessesByUserId(UserData.GetUserId());
                #endregion

                #region Getting form ids available for User
                //Form Ids where user has Global accesses
                IEnumerable<long> formIdsWithGlobalAccess = _definitionRepository.GetFormIdsWhereGlobalAccess(globalAccesses);

                //Form Ids where user has Local access
                IEnumerable<long> formIdsWithLocalAccess = _formRepository.GetFormIdsWhereLocalAccess(UserData.GetUserId());

                //Form Ids where user has any Participation
                IEnumerable<long> formIdsWithParticipation = _definitionRepository.GetFormIdsWhereParticipation(UserData.GetUserId());

                //Form Ids unioning and sorting
                List<long> availableFormIds = formIdsWithParticipation.Union(formIdsWithLocalAccess)
                                                                      .Union(formIdsWithGlobalAccess)
                                                                      .OrderBy(id => id)
                                                                      .ToList();
                #endregion
                UserData.AvailableFormIds = availableFormIds;

                #region Get forms from Database and prepare available data
                List<Form> availableForms = _formRepository.GetForms(availableFormIds);

                FormDataExtractor formDataExtractor = new FormDataExtractor();
                FormDataAvailable formDataAvailable = new FormDataAvailable(formDataExtractor);

                formDataAvailable.PrepareAvailableFormPermissions(UserData.GetUserId(),
                                                                  availableForms,
                                                                  formIdsWithGlobalAccess,
                                                                  formIdsWithLocalAccess,
                                                                  formIdsWithParticipation);
                formDataAvailable.PrepareAvailablePermissions(availableForms);
                formDataAvailable.PrepareAvailableEmployees(availableForms);
                formDataAvailable.PrepareAvailablePeriods(availableForms);
                formDataAvailable.PrepareAvailableYears(availableForms);
                formDataAvailable.PrepareAvailableDepartments(availableForms);
                formDataAvailable.PrepareAvailableTeams(availableForms);
                formDataAvailable.PrepareAvailableWorkprojects(availableForms);
                #endregion

                #region Prepare user selections
                UserSelectionsHandler userSelectionsHandler = new UserSelectionsHandler(formDataAvailable);
                userSelectionsHandler.PrepareSelections(userSelections);
                #endregion

                #region Prepare TableRows: table's content
                TableRowsCreator tableRowsCreator = new TableRowsCreator(formDataAvailable, userSelections);
                List<TableRowDTO> tableRows = tableRowsCreator.CreateTableRows();
                #endregion

                #region Prepare DropdownLists: dropdown's content
                DropdownListsCreator dropdownListsCreator = new DropdownListsCreator(formDataAvailable, userSelections);
                DropdownListsDTO dropdownLists = new DropdownListsDTO
                {
                    EmployeeDropdownList = dropdownListsCreator.CreateEmployeeDropdownLists(),
                    PeriodDropdownList = dropdownListsCreator.CreatePeriodDropdownLists(),
                    YearDropdownList = dropdownListsCreator.CreateYearDropdownLists(),
                    PermissionDropdownList = dropdownListsCreator.CreatePermissionDropdownLists(),
                    DepartmentDropdownList = dropdownListsCreator.CreateDepartmentDropdownLists(),
                    TeamDropdownList = dropdownListsCreator.CreateTeamDropdownLists(),
                    WorkprojectDropdownList = dropdownListsCreator.CreateWorkprojectDropdownLists(),
                };
                #endregion

                return new FormIndexDTO
                {
                    TableRows = tableRows,
                    DropdownLists = dropdownLists,
                };
            }
            catch (ValidationException ex)
            {
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to prepare available forms. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public FormDTO GetFormDTO(long formId)
        {
            try
            {
                FormDTO formDTO = _mapper.Map<FormDTO>(_formRepository.GetForm(formId));
                PrepareThresholdTargetChallangeForPresentation(formDTO);
                RoundOverallKpiForPresentation(formDTO);
                RoundKpiForPresentation(formDTO);
                return formDTO;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to perform operation. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public FormDTO GetPrefilledFormDTO(long formId)
        {
            try
            {
                FormDTO formDTO = _mapper.Map<FormDTO>(_formRepository.GetForm(formId));

                formDTO.Id = 0;
                formDTO.LastSavedBy = string.Empty;
                formDTO.LastSavedAt = DateTime.Now;
                formDTO.AreObjectivesFrozen = false;
                formDTO.AreResultsFrozen = false;
                formDTO.Definition.Id = 0;
                formDTO.Conclusion = new ConclusionDTO();
                formDTO.Signatures = new SignaturesDTO();
                for (int i = 0; i < formDTO.ObjectivesResults.Count(); i++)
                {
                    formDTO.ObjectivesResults[i].Id = 0;
                    formDTO.ObjectivesResults[i].Result.Achieved = string.Empty;
                    formDTO.ObjectivesResults[i].Result.Kpi = string.Empty;
                }

                PrepareThresholdTargetChallangeForPresentation(formDTO);
                return formDTO;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to create form based on selected one. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public FormDTO GetIsFrozenStates(long formId)
        {
            try
            {
                return _mapper.Map<FormDTO>(_formRepository.GetStates(formId));
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to receive form data. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public SignaturesDTO GetSignaturesDTO(long signaturesId)
        {
            try
            {
                return _mapper.Map<SignaturesDTO>(_signaturesRepository.GetSignatures(signaturesId));
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to receive signatures data. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public Dictionary<string, string> GetWorkprojectIdsNames()
        {
            try
            {
                return _workprojectRepository.GetWorkprojectsNames()
                                             .OrderBy(wp => wp.Id)
                                             .ToDictionary(w => w.Id.ToString(),
                                                           w => w.Name);
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to receive workprojects names. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public Dictionary<string, string> GetUserIdsNames()
        {
            try
            {
                return _userRepository.GetUsersNames()
                                      .OrderBy(u => u.LastNameEng)
                                      .ToDictionary(u => u.Id.ToString(),
                                                    u => $"{u.LastNameEng} {u.FirstNameEng}");
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to receive users names. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public List<string> GetPeriodNames()
        {
            return Enum.GetNames(typeof(Periods)).ToList();
        }
        public string GetWorkprojectDescription(long workprojectId)
        {
            try
            {
                string? description = _workprojectRepository.GetWorkprojectData(workprojectId).Description;
                return description == null ? string.Empty : description;
            }
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to receive workproject description. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }
        public EmployeeDTO GetEmployeeDTO(long userId)
        {
            try
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
            catch (Exception ex) when (ex is ArgumentNullException ||
                                       ex is InvalidOperationException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to receive user data. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }

        public Dictionary<string, object> UpdateAndReturnSignatures(long formId,
                                                                    string checkboxId,
                                                                    bool isCheckboxChecked)
        {
            if (string.IsNullOrEmpty(checkboxId))
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Signature data is not affected.");

            try
            {
                PropertyLinkerHandler propertyLinkerHandler = new PropertyLinkerHandler();
                PropertyLinkerFactory propertyLinkerFactory = new PropertyLinkerFactory();

                #region Determine which properties were affected. Getting affected PropertyLinker
                foreach (PropertyType type in Enum.GetValues(typeof(PropertyType)).Cast<PropertyType>())
                {
                    IPropertyLinker propertyLinker = propertyLinkerFactory.CreatePropertyLinker(type);
                    if (propertyLinkerHandler.IsPropertyLinkerAffected(propertyLinker, checkboxId))
                        break;
                }

                if (propertyLinkerHandler.AffectedPropertyLinker == null)
                {
                    throw new ValidationException($"Signature process is not possible. " +
                                                  $"Neither objectives nor results are involved into signature process.");
                }
                #endregion

                #region Get property-value pairs which should be saved in Database
                Dictionary<string, object> propertiesValues =
                    propertyLinkerHandler.GetPropertiesValues(checkboxId, isCheckboxChecked);

                if (propertiesValues.Count == 0)
                {
                    throw new ValidationException($"Signature process is not possible. " +
                                                  $"Signature data is not affected.");
                }
                #endregion

                #region Get form from database and check signature possibility
                Form statesAndSignatures = _formRepository.GetStatesAndSignatures(formId);
                FormDataHandler formDataHandler = new FormDataHandler();

                if (propertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForObjectives &&
                   !formDataHandler.IsObjectivesSignaturePossible(statesAndSignatures))
                {
                    throw new ValidationException($"Signature process is not possible. " +
                                                  $"Objectives should be frozen at first.");
                }

                if (propertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForResults &&
                   !formDataHandler.IsResultsSignaturePossible(statesAndSignatures))
                {
                    throw new ValidationException($"Signature process is not possible. " +
                                                  $"Results should be frozen at first.");
                }
                #endregion

                #region Fill property-value pair with User signature and Update Form data
                formDataHandler.PutUserSignature(ref propertiesValues);
                formDataHandler.UpdateSignatures(statesAndSignatures, propertiesValues);
                formDataHandler.UpdateLastSavedFormData(statesAndSignatures);

                _formRepository.UpdateSignatures(statesAndSignatures);
                return propertiesValues;
            }
            catch (ValidationException ex)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException ||
                                       ex is ArgumentException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to update signatures. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
            #endregion
        }
        public void UpdateForm(long formId,
                               DefinitionDTO definitionDTO,
                               ConclusionDTO conclusionDTO,
                               List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            try
            {
                if (formId <= 0)
                    throw new ValidationException($"Unable to perform operation. Unknown form.");

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
                    List<ObjectiveResult> objectiveResults = _objectiveResultRepository.GetObjectivesResults(formId);
                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.HandleResultsUpdateProcess(objectiveResults, objectiveResultDTOs);

                    ConclusionHandler conclusionHandler = new ConclusionHandler(conclusionDTO, objectiveResultDTOs);
                    conclusionHandler.HandleUpdateProcess();

                    Form form = new Form
                    {
                        Id = formId,
                        LastSavedAt = DateTime.Now,
                        LastSavedBy = UserData.GetUserName(),
                        Conclusion = _mapper.Map<Conclusion>(conclusionDTO),
                        ObjectivesResults = objectiveResults,
                    };

                    int index = 0;
                    foreach (var objRes in form.ObjectivesResults)
                    {
                        if (index < objectiveResultDTOs.Count())
                            objRes.Result = _mapper.Map<Result>(objectiveResultDTOs[index].Result);
                        index++;
                    }

                    _formRepository.UpdateResultsConclusion(form);
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
                                       ex is InvalidOperationException ||
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
                throw;
            }
            catch (Exception ex) when (ex is AutoMapperMappingException ||
                                       ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
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
        /// Creates a new form with same Definition and Objecties as Form with formId
        /// but with the next Period
        /// </summary>
        /// <param name="formId"></param>
        /// <returns>string representing an error message</returns>
        public string PromoteForm(long formId)
        {
            try
            {
                Promoter promoter = new Promoter(_formRepository,
                                                 _definitionRepository);
                
                Form newForm = promoter.GetPromotedForm(formId);

                _formRepository.CreateForm(newForm);
                return string.Empty;
            }
            catch (ValidationException ex)
            {
                return ex.Message;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");
                return "Unable to promote form. " +
                       "Try again, and if the problem persists, " +
                       "see your system administrator.";
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
                    if (statesAndSignatures.AreObjectivesFrozen ||
                        statesAndSignatures.AreResultsFrozen)
                    {
                        return;
                    }

                    // check if Definition is ready to be frozen
                    Definition definition = _definitionRepository.GetDefinition(formId);
                    DefinitionHandler definitionValidator = new DefinitionHandler();
                    definitionValidator.HandleChangeStateProcess(definition);

                    // check if Objectives are ready to be frozen
                    List<ObjectiveResult> objectiveResults = _objectiveResultRepository.GetObjectivesResults(formId);
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
                    if (!statesAndSignatures.AreObjectivesFrozen ||
                        statesAndSignatures.AreResultsFrozen)
                    {
                        return;
                    }

                    if (!statesAndSignatures.Signatures.AreObjectivesSigned)
                    {
                        throw new ValidationException("Unable to freeze results. Objectives are not signed.");
                    }

                    // check if Results are ready to be frozen
                    List<ObjectiveResult> objectiveResults = _objectiveResultRepository.GetObjectivesResults(formId);
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
                        return;
                    }

                    // drop all signatures
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
                    if (!statesAndSignatures.AreObjectivesFrozen ||
                        !statesAndSignatures.AreResultsFrozen)
                    {
                        return;
                    }

                    // drop signatures for Results only
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
            }
            catch (ValidationException ex)
            {
                throw;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                throw new ValidationException("Unable to change state of the Form. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.");
            }
        }

        public string DeleteForm(long formId)
        {
            try
            {
                _formRepository.DeleteForm(formId);
                _logger.LogInformation($"Form with id={formId} was deleted.\n" +
                                       $"Operation was performed by user: id={UserData.GetUserId()}, name={UserData.GetUserName()}.");
                return string.Empty;
            }
            catch (Exception ex) when (ex is InvalidOperationException ||
                                       ex is ArgumentNullException ||
                                       ex is DbUpdateException)
            {
                _logger.LogError($"Message: {ex.Message}.\n" +
                                 $"StackTrace: {ex.StackTrace}.\n" +
                                 $"TargetSite = {ex.TargetSite}.\n");

                return "Unable to delete form. " +
                       "Try again, and if the problem persists, " +
                       "see your system administrator.";
            }
        }


        private void PrepareThresholdTargetChallangeForPresentation(FormDTO formDTO)
        {
            if (formDTO.ObjectivesResults == null)
                return;

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
        }

        private void RoundOverallKpiForPresentation(FormDTO formDTO)
        {
            if (formDTO.Conclusion == null ||
                formDTO.Conclusion.OverallKpi == null)
                return;

            formDTO.Conclusion.OverallKpi = Math.Round((double)formDTO.Conclusion.OverallKpi, 2);
        }

        private void RoundKpiForPresentation(FormDTO formDTO)
        {
            if (formDTO.ObjectivesResults == null)
                return;

            for (int i = 0; i < formDTO.ObjectivesResults.Count; i++)
            {
                if (string.IsNullOrEmpty(formDTO.ObjectivesResults[i].Result.Kpi))
                {
                    continue;
                }

                if (double.TryParse(formDTO.ObjectivesResults[i].Result.Kpi, out double kpi))
                {
                    formDTO.ObjectivesResults[i].Result.Kpi = Math.Round(kpi, 2).ToString();
                }
            }
        }
    }
}
