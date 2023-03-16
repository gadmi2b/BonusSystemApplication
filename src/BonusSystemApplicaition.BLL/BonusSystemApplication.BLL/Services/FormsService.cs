using AutoMapper;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.Processes.Filtering;
using BonusSystemApplication.BLL.Processes.Signing;
using BonusSystemApplication.BLL.UserIdentiry;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BonusSystemApplication.BLL.Processes;

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
                            IWorkprojectRepository workprojectRepo,
                            IDefinitionRepository definitionRepo,
                            IConclusionRepository conclusionRepo,
                            ISignaturesRepository signaturesRepo,
                            IObjectiveResultRepository objectiveResultRepo,
                            IGlobalAccessRepository globalAccessRepo)
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
            #region Global accesses for user
            IEnumerable<GlobalAccess> globalAccesses = _globalAccessRepository.GetGlobalAccessesByUserId(UserData.UserId);
            #endregion

            #region Getting form Ids available for User
            //Form Ids where user has Global accesses
            IEnumerable<long> formIdsWithGlobalAccess = _definitionRepository.GetFormIdsWhereGlobalAccess(globalAccesses);

            //Form Ids where user has Local access
            IEnumerable<long> formIdsWithLocalAccess = _formRepository.GetFormIdsWhereLocalAccess(UserData.UserId);

            //Form Ids where user has any Participation
            IEnumerable<long> formIdsWithParticipation = _definitionRepository.GetFormIdsWhereParticipation(UserData.UserId);

            //Form Ids unioning and sorting
            List<long> availableFormIds = formIdsWithParticipation.Union(formIdsWithLocalAccess)
                                                                  .Union(formIdsWithGlobalAccess)
                                                                  .OrderBy(id => id)
                                                                  .ToList();
            #endregion

            #region Load data from database into forms and get corrsponding permissions
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
                    LastSavedDateTime = pair.Key.LastSavedDateTime,
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
                Form form = _formRepository.GetForm(formId);
                formDTO = _mapper.Map<FormDTO>(_formRepository.GetForm(formId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: {nameof(GetFormDTO)}. Params: {nameof(formId)} = {formId} " +
                                 $"EF msg: {ex.Message}", "");
                throw new ValidationException("Unable to get form data. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.", "");
            }

            PrepareFormDTOForPresentation(formDTO);
            return formDTO;
        }
        public FormDTO GetIsFreezedStates(long formId)
        {
            FormDTO formDTO = new FormDTO();
            try
            {
                formDTO = _mapper.Map<FormDTO>(_formRepository.GetStates(formId));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: {nameof(GetIsFreezedStates)}. Params: {nameof(formId)} = {formId} " +
                                 $"EF msg: {ex.Message}", "");
                throw new ValidationException("Unable to get form data. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.", "");
            }

            return formDTO;
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


        public Dictionary<string,string> GetUsersNames()
        {
            return _userRepository.GetUsersNames()
                                  .ToDictionary(u => u.Id.ToString(),
                                                u => $"{u.LastNameEng} {u.FirstNameEng}");
        }
        public Dictionary<string, string> GetWorkprojectsNames()
        {
            return _workprojectRepository.GetWorkprojectsNames()
                                  .ToDictionary(w => w.Id.ToString(),
                                                 w => w.Name);
        }
        public Dictionary<string, string> GetPeriodsNames()
        {
            return Enum.GetNames(typeof(Periods))
                                  .ToDictionary(s => s,
                                                s => s);
        }


        public string GetWorkprojectDescription(long workprojectId)
        {
            string? description = _workprojectRepository.GetWorkprojectData(workprojectId).Description;
            if (description != null) return description;
            else return string.Empty;
        }
        public EmployeeDTO GetEmployeeDTO(long userId)
        {
            return _mapper.Map<EmployeeDTO>(_userRepository.GetUserData(userId));
        }

        public Dictionary<string, object> UpdateAndReturnSignatures(long formId,
                                                                    string checkboxId,
                                                                    bool isCheckboxChecked)
        {
            if (string.IsNullOrEmpty(checkboxId))
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Signature data is not affected.", "");
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
                                              $"Neither objectives nor results are involved into signature process.", "");
            }
            #endregion

            #region Get property-value pairs which should be saved in Database
            Dictionary<string, object> propertiesValues =
                PropertyLinkerHandler.GetPropertiesValues(checkboxId, isCheckboxChecked);

            if (propertiesValues.Count == 0)
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Signature data is not affected.", "");
            }
            #endregion

            #region Get form from database and check signature possibility
            Form statesAndSignatures = _formRepository.GetStatesAndSignatures(formId);

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForObjectives &&
               !FormDataHandler.IsObjectivesSignaturePossible(statesAndSignatures))
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Objectives should be freezed at first.", "");
            }

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForResults &&
               !FormDataHandler.IsResultsSignaturePossible(statesAndSignatures))
            {
                throw new ValidationException($"Signature process is not possible. " +
                                              $"Results should be freezed at first.", "");
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
            catch (DbUpdateException ex)
            {
                _logger.LogError($"{nameof(FormsService)}. {nameof(UpdateAndReturnSignatures)}. " +
                                 $"EF error message: {ex.Message}");
                throw new ValidationException("Unable to save changes. " +
                                              "Try again, and if the problem persists, " +
                                              "Contact your system administrator.", "");
            }
            catch (Exception ex)
            {
                // TODO: add error handling

                throw new Exception($"{nameof(FormsService)}. {nameof(UpdateAndReturnSignatures)}. Not handled commnon exception: " +
                                    $"{ex.Message}");
                //return new Dictionary<string, object>();
            }

            #endregion

            return propertiesValues;
        }

        public void UpdateForm(long formId,
                               DefinitionDTO definition,
                               ConclusionDTO conclusion,
                               SignaturesDTO signatures,
                               IList<ObjectiveResultDTO> objectivesResults)
        {
            // TODO: check what could be saved: formViewModel.IsStates and Signatures (see SaveProcess())
            //       launch Update (formViewModel, PartsToSave) method:
            //       if Id == 0: provide checks for: Definition, Objectives, Results (recalculate based on Objectives of this formViewModel)
            //       if id != 0: get Form with Id,
            //                   provide checks for: Definition, Objectives, Results (recalculate based on Objectives of loaded Form data)
            if (formId == 0)
            {
                return;
            }

            FormDTO statesAndSignatures = _mapper.Map<FormDTO>(_formRepository.GetStatesAndSignatures(formId));

            if (statesAndSignatures.Signatures.IsResultsSigned)
            {
                // Nothing could be saved
                return;
            }
            else if (statesAndSignatures.IsResultsFreezed)
            {
                // Conclusion's comments could be saved
                // nothing to check
                _formRepository.UpdateConclusionComments(new Form
                {
                    LastSavedBy = UserData.GetUserName(),
                    LastSavedDateTime = DateTime.Now,
                    Conclusion = new Conclusion
                    {
                        ManagerComment = conclusion.ManagerComment,
                        EmployeeComment = conclusion.EmployeeComment,
                        OtherComment = conclusion.OtherComment,
                    }
                });
            }
            else if (statesAndSignatures.Signatures.IsObjectivesSigned ||
                     statesAndSignatures.IsObjectivesFreezed)
            {
                // Results & Conclusion could be saved
                // TODO: check Results and Conclustion
                _formRepository.UpdateResultsConclusion(new Form
                {
                    LastSavedDateTime = DateTime.Now,
                    LastSavedBy = UserData.GetUserName(),
                    Conclusion = _mapper.Map<Conclusion>(conclusion),
                    ObjectivesResults = _mapper.Map<List<ObjectiveResult>>(objectivesResults),
                });
            }
            else
            {
                // Definition & Objectives & Results & Conclusion could be saved:
                // Definition & Objectives are taken from viewmodel
                // Results: achieved is taken from viewmodel, others recalculated
                // Conclusion: Comments are taken from viewmodel, IsProposalForBonusPayment and OverallKPI recalculated

                // TODO: validate definition


                _formRepository.UpdateDefinitionObjectivesResultsConclusion(new Form
                {
                    LastSavedDateTime = DateTime.Now,
                    LastSavedBy = UserData.GetUserName(),

                    Definition = _mapper.Map<Definition>(definition),
                    Conclusion = _mapper.Map<Conclusion>(conclusion),
                    ObjectivesResults = _mapper.Map<List<ObjectiveResult>>(objectivesResults),
                });
            }

            // TODO: validate saved objects
            // TODO: update tables

        }

        public void CreateForm(DefinitionDTO definition,
                               IList<ObjectiveResultDTO> objectivesResults)
        {
            try
            {
                long formId = 0;
                DefinitionValidator definitionValidator = new DefinitionValidator(formId,
                                                                                  _mapper,
                                                                                  _userRepository,
                                                                                  _definitionRepository,
                                                                                  _workprojectRepository,
                                                                                  definition);
                definitionValidator.ValidateCreateProcess();

                _formRepository.CreateForm(new Form
                {
                    Id = 0,
                    LastSavedBy = UserData.GetUserName(),
                    LastSavedDateTime = DateTime.Now,
                    Definition = _mapper.Map<Definition>(definition),
                    ObjectivesResults = _mapper.Map<List<ObjectiveResult>>(objectivesResults),
                    Conclusion = new Conclusion(),
                    Signatures = new Signatures(),
                });
            }
            catch (ValidationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Method: {nameof(CreateForm)}. " +
                                 $"Message: {ex.Message}", "");
                throw new ValidationException("Unable to create new form. " +
                                              "Try again, and if the problem persists, " +
                                              "see your system administrator.", "");
            }
        }

        /// <summary>
        /// In according to request changes IsFreezed states for Form.
        /// Before Unfreezing states it drops collected signatures as well.
        /// </summary>
        /// <param name="act">freeze or unfreeze</param>
        /// <param name="type">objectives or results</param>
        /// <param name="formId">id of Form to operate</param>
        /// <exception cref="ValidationException"></exception>
        public void ChangeState(string act,
                                string type,
                                long formId)
        {
            FormDTO formDTO = _mapper.Map<FormDTO>(_formRepository.GetStates(formId));
            formDTO.LastSavedDateTime = DateTime.Now;
            formDTO.LastSavedBy = UserData.GetUserName();

            #region Freezing Objectives/Results
            if (act == "freeze" && type == "objectives")
            {
                if (formDTO.IsObjectivesFreezed)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                       $"Params: " +
                                       $"{nameof(formId)} = {formId}, " +
                                       $"{nameof(act)} = {act}, " +
                                       $"{nameof(type)} = {type}. ",
                                       $"Unable to freeze Objectives. Objectives are already freezed. ", "");
                    return;
                }

                if (formDTO.IsResultsFreezed)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                       $"Params: " +
                                       $"{nameof(formId)} = {formId}, " +
                                       $"{nameof(act)} = {act}, " +
                                       $"{nameof(type)} = {type}. ",
                                       $"Unable to freeze Objectives. Results are already freezed. ", "");
                    return;
                }

                // check if Definition is ready to be freezed
                DefinitionValidator definitionValidator = new DefinitionValidator(formId, _mapper, _definitionRepository);
                try
                {
                    definitionValidator.ValidateChangeStateProcess();
                }
                catch (ValidationException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {

                }
                // TODO: check if Objectives are ready to be freezed
                //       at least n'th objectives are filled



                    formDTO.IsObjectivesFreezed = true;
                _formRepository.UpdateStates(_mapper.Map<Form>(formDTO));
                return;
            }

            if (act == "freeze" && type == "results")
            {
                if (!formDTO.IsObjectivesFreezed)
                {
                    _logger.LogWarning($"{nameof(FormsService)}. Wrong client side's call of Method: {nameof(ChangeState)}. " +
                                       $"Unable to freeze Results. Objectives are not freezed. " +
                                       $"{nameof(formId)} = {formId}, {nameof(act)} = {act}, {nameof(type)} = {type}", "");
                    return;
                }

                if (formDTO.IsResultsFreezed)
                {
                    _logger.LogWarning($"{nameof(FormsService)}. Wrong client side's call of Method: {nameof(ChangeState)}. " +
                                       $"Unable to freeze Results. Results are already freezed. " +
                                       $"{nameof(formId)} = {formId}, {nameof(act)} = {act}, {nameof(type)} = {type}", "");
                    return;
                }

                // TODO: check if Results are ready to be freezed
                //       each result for each objective is filled properly

                formDTO.IsResultsFreezed = true;
                try
                {
                    _formRepository.UpdateStates(_mapper.Map<Form>(formDTO));
                }
                catch(Exception ex)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. " +
                                       $"Called method: {nameof(_formRepository.UpdateStates)}. " +
                                       $"Params: " +
                                       $"{nameof(formDTO.Id)} = {formDTO.Id}, " +
                                       $"{nameof(formDTO.LastSavedBy)} = {formDTO.LastSavedBy}, " +
                                       $"{nameof(formDTO.LastSavedDateTime)} = {formDTO.LastSavedDateTime}, " +
                                       $"{nameof(formDTO.IsObjectivesFreezed)} = {formDTO.IsObjectivesFreezed}, " +
                                       $"{nameof(formDTO.IsResultsFreezed)} = {formDTO.IsResultsFreezed}. " +
                                       $"Message: {ex.Message}", "");

                    throw new ValidationException("Unable to change state of form. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.", "");
                }

                return;
            }
            #endregion

            #region Unfreezing Objectives/Results
            if (act == "unfreeze" && type == "objectives")
            {
                if (!formDTO.IsObjectivesFreezed)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                       $"Params: " +
                                       $"{nameof(formId)} = {formId}, " +
                                       $"{nameof(act)} = {act}, " +
                                       $"{nameof(type)} = {type}. ",
                                       $"Unable to unfreeze Objectives. Objectives are not freezed. ", "");
                    return;
                }

                // Drop all signatures
                try
                {
                    _signaturesRepository.DropSignatures(formId);
                }
                catch(Exception ex)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. " +
                                       $"Called method: {nameof(_signaturesRepository.DropSignatures)}. " +
                                       $"Params: " +
                                       $"{nameof(formId)} = {formId}. " +
                                       $"Message: {ex.Message}", "");

                    throw new ValidationException("Unable to change state of form. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.", "");
                }

                formDTO.IsObjectivesFreezed = false;
                formDTO.IsResultsFreezed = false;       // results are also should be unfreezed

                _formRepository.UpdateStates(_mapper.Map<Form>(formDTO));
                return;
            }

            if (act == "unfreeze" && type == "results")
            {
                if (!formDTO.IsObjectivesFreezed)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                       $"Params: " +
                                       $"{nameof(formId)} = {formId}, " +
                                       $"{nameof(act)} = {act}, " +
                                       $"{nameof(type)} = {type}. " +
                                       $"Unable to unfreeze Results. Objectives are not freezed. ", "");
                    return;
                }

                if (!formDTO.IsResultsFreezed)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. Wrong client side's call. " +
                                       $"Params: " +
                                       $"{nameof(formId)} = {formId}, " +
                                       $"{nameof(act)} = {act}, " +
                                       $"{nameof(type)} = {type}. " +
                                       $"Unable to unfreeze Results. Results are not freezed. ", "");
                    return;
                }

                // Drop signatures for results only
                try
                {
                    _signaturesRepository.DropSignaturesForResults(formId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Method: {nameof(ChangeState)}. " +
                                       $"Called method: {nameof(_signaturesRepository.DropSignaturesForResults)}. " +
                                       $"Params: " +
                                       $"{nameof(formId)} = {formId}. " +
                                       $"Message: {ex.Message}", "");

                    throw new ValidationException("Unable to change state of form. " +
                                                  "Try again, and if the problem persists, " +
                                                  "see your system administrator.", "");
                }

                formDTO.IsResultsFreezed = false;
                _formRepository.UpdateStates(_mapper.Map<Form>(formDTO));
                return;
            }
            #endregion

            _logger.LogWarning($"Method: {nameof(ChangeState)}. Unknown call. " +
                               $"Params: {nameof(act)} = {act}, {nameof(type)} = {type}.");
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
