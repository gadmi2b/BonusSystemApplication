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

        public async Task<FormIndexDTO> GetFormIndexDtoAsync(UserSelectionsDTO userSelections)
        {
            try
            {
                #region Getting global accesses user has
                IEnumerable<GlobalAccess> globalAccesses = await _globalAccessRepository.GetGlobalAccessesByUserIdAsync(UserData.GetUserId());
                #endregion

                #region Getting form ids available for User
                //Form Ids where user has Global accesses
                IEnumerable<long> formIdsWithGlobalAccess = await _definitionRepository.GetFormIdsWhereGlobalAccessAsync(globalAccesses);

                //Form Ids where user has Local access
                IEnumerable<long> formIdsWithLocalAccess = await _formRepository.GetFormIdsWhereLocalAccessAsync(UserData.GetUserId());

                //Form Ids where user has any Participation
                IEnumerable<long> formIdsWithParticipation = await _definitionRepository.GetFormIdsWhereParticipationAsync(UserData.GetUserId());

                //Form Ids unioning and sorting
                List<long> availableFormIds = formIdsWithParticipation.Union(formIdsWithLocalAccess)
                                                                      .Union(formIdsWithGlobalAccess)
                                                                      .OrderBy(id => id)
                                                                      .ToList();
                #endregion
                
                UserData.AvailableFormIds = availableFormIds;

                #region Get forms from Database and prepare available data
                List<Form> availableForms = await _formRepository.GetFormsAsync(availableFormIds);

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
        public async Task<FormDTO> GetFormDtoAsync(long formId)
        {
            try
            {
                FormDTO formDTO = _mapper.Map<FormDTO>(await _formRepository.GetFormAsync(formId));
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
        public async Task<FormDTO> GetPrefilledFormDtoAsync(long formId)
        {
            try
            {
                FormDTO formDTO = _mapper.Map<FormDTO>(await _formRepository.GetFormAsync(formId));

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
        public async Task<FormDTO> GetIsFrozenStatesAsync(long formId)
        {
            try
            {
                return _mapper.Map<FormDTO>(await _formRepository.GetStatesAsync(formId));
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
        public async Task<SignaturesDTO> GetSignaturesDtoAsync(long signaturesId)
        {
            try
            {
                return _mapper.Map<SignaturesDTO>(await _signaturesRepository.GetSignaturesAsync(signaturesId));
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
        public async Task<Dictionary<string, string>> GetWorkprojectIdsNamesAsync()
        {
            try
            {
                List<Workproject> workprojects = await _workprojectRepository.GetWorkprojectsNamesAsync();
                return workprojects.OrderBy(wp => wp.Id)
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
        public async Task<Dictionary<string, string>> GetUserIdsNamesAsync()
        {
            try
            {
                List<User> users = await _userRepository.GetUsersNamesAsync();
                return users.OrderBy(u => u.LastNameEng)
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
        public async Task<string> GetWorkprojectDescriptionAsync(long workprojectId)
        {
            try
            {
                var workproject = await _workprojectRepository.GetWorkprojectDataAsync(workprojectId);
                return workproject.Description == null ? string.Empty : workproject.Description;
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
        public async Task<EmployeeDTO> GetEmployeeDtoAsync(long userId)
        {
            try
            {
                User userData = await _userRepository.GetUserDataAsync(userId);
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

        public async Task<Dictionary<string, object>> UpdateAndReturnSignaturesAsync(long formId,
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
                Form statesAndSignatures = await _formRepository.GetStatesAndSignaturesAsync(formId);
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

                await _formRepository.UpdateSignaturesAsync(statesAndSignatures);
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
        public async Task UpdateFormAsync(long formId,
                                          DefinitionDTO definitionDTO,
                                          ConclusionDTO conclusionDTO,
                                          List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            try
            {
                if (formId <= 0)
                    throw new ValidationException($"Unable to perform operation. Unknown form.");

                Form statesAndSignatures = await _formRepository.GetStatesAndSignaturesAsync(formId);
                if (statesAndSignatures.Signatures.AreResultsSigned)
                    throw new ValidationException("Unable to perform operation. Results are already signed.");

                if (statesAndSignatures.AreResultsFrozen)
                {
                    #region Conclusion's comments could be updated
                    await _formRepository.UpdateConclusionCommentsAsync(new Form
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
                    List<ObjectiveResult> objectiveResults = await _objectiveResultRepository.GetObjectivesResultsAsync(formId);
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

                    await _formRepository.UpdateResultsConclusionAsync(form);
                    #endregion
                }
                else
                {
                    #region Definition & Objectives & Results & Conclusion could be updated
                    DefinitionHandler definitionHandler = new DefinitionHandler(formId,
                                                                                _userRepository,
                                                                                _definitionRepository,
                                                                                _workprojectRepository);
                    await definitionHandler.HandleUpdateProcessAsync(definitionDTO);

                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.HandleObjectivesUpdateProcess(objectiveResultDTOs);

                    ConclusionHandler conclusionHandler = new ConclusionHandler(conclusionDTO, objectiveResultDTOs);
                    conclusionHandler.HandleUpdateProcess();

                    await _formRepository.UpdateDefinitionObjectivesResultsConclusionAsync(new Form
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

        public async Task CreateFormAsync(DefinitionDTO definitionDTO,
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
                await definitionHandler.HandleUpdateProcessAsync(definitionDTO);

                ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                objectivesResultsHandler.HandleObjectivesUpdateProcess(objectiveResultDTOs);

                ConclusionHandler conclusionHandler = new ConclusionHandler(conclusionDTO, objectiveResultDTOs);
                conclusionHandler.HandleUpdateProcess();

                await _formRepository.CreateFormAsync(new Form
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
        public async Task<string> PromoteFormAsync(long formId)
        {
            try
            {
                Promoter promoter = new Promoter(_formRepository,
                                                 _definitionRepository);
                
                Form newForm = await promoter.GetPromotedFormAsync(formId);

                await _formRepository.CreateFormAsync(newForm);
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
        public async Task ChangeStateAsync(long formId,
                                           string changeToState,
                                           string objectivesOrResults)
        {
            try
            {
                Form statesAndSignatures = await _formRepository.GetStatesAndSignaturesAsync(formId);

                #region Freezing Objectives
                if (changeToState == "frozen" && objectivesOrResults == "objectives")
                {
                    if (statesAndSignatures.AreObjectivesFrozen ||
                        statesAndSignatures.AreResultsFrozen)
                    {
                        return;
                    }

                    // check if Definition is ready to be frozen
                    Definition definition = await _definitionRepository.GetDefinitionAsync(formId);
                    DefinitionHandler definitionValidator = new DefinitionHandler();
                    definitionValidator.HandleChangeStateProcess(definition);

                    // check if Objectives are ready to be frozen
                    List<ObjectiveResult> objectiveResults = await _objectiveResultRepository.GetObjectivesResultsAsync(formId);
                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.ValidateObjectivesChangeStateProcess(objectiveResults);

                    await _formRepository.UpdateStatesAsync(new Form
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
                    List<ObjectiveResult> objectiveResults = await _objectiveResultRepository.GetObjectivesResultsAsync(formId);
                    ObjectivesResultsHandler objectivesResultsHandler = new ObjectivesResultsHandler();
                    objectivesResultsHandler.ValidateResultsChangeStateProcess(objectiveResults);

                    await _formRepository.UpdateStatesAsync(new Form
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
                    await _signaturesRepository.DropSignaturesAsync(formId);

                    await _formRepository.UpdateStatesAsync(new Form
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
                    await _signaturesRepository.DropSignaturesForResultsAsync(formId);

                    await _formRepository.UpdateStatesAsync(new Form
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

        public async Task<string> DeleteFormAsync(long formId)
        {
            try
            {
                await _formRepository.DeleteFormAsync(formId);
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
