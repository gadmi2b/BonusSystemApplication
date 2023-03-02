using AutoMapper;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.BLL.Interfaces;
using BonusSystemApplication.BLL.Processes.Filtering;
using BonusSystemApplication.BLL.Processes.Signing;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Repositories;
using BonusSystemApplication.UserIdentiry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

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

            UserData.UserId = 7;
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
            return _mapper.Map<FormDTO>(_formRepository.GetIsFreezedStates(formId));
        }


        public DefinitionDTO GetDefinitionDTO(long formId)
        {
            return _mapper.Map<DefinitionDTO>(_definitionRepository.GetDefinition(formId));
        }
        public ConclusionDTO GetConclusionDTO(long formId)
        {
            return _mapper.Map<ConclusionDTO>(_conclusionRepository.GetConclusion(formId));
        }
        public SignaturesDTO GetSignaturesDTO(long formId)
        {
            return _mapper.Map<SignaturesDTO>(_signaturesRepository.GetSignatures(formId));
        }
        public IList<ObjectiveResultDTO> GetObjectivesResultsDTO(long formId)
        {
            return _mapper.Map<IList<ObjectiveResultDTO>>(_objectiveResultRepository.GetObjectivesResults(formId));
        }


        public List<SelectListItem> GetUsersNames()
        {
            return _userRepository.GetUsersNames()
                                  .Select(u => new SelectListItem
                                  {
                                      Value = u.Id.ToString(),
                                      Text = $"{u.LastNameEng} {u.FirstNameEng}",
                                  })
                                  .ToList();
        }
        public List<SelectListItem> GetWorkprojectsNames()
        {
            return _workprojectRepository.GetWorkprojectsNames()
                                  .Select(w => new SelectListItem
                                  {
                                      Value = w.Id.ToString(),
                                      Text = w.Name,
                                  })
                                  .ToList();
        }
        public List<SelectListItem> GetPeriodsNames()
        {
            return Enum.GetNames(typeof(Periods))
                                  .Select(s => new SelectListItem
                                  {
                                      Value = s,
                                      Text = s,
                                  })
                                  .ToList();
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
                throw new ValidationException($"{DateTime.Now}: Signature process is not possible. " +
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
                throw new ValidationException($"{DateTime.Now}: Signature process is not possible. " +
                                              $"Neither objectives nor results are involved into signature process.", "");
            }
            #endregion

            #region Get property-value pairs which should be saved in Database
            Dictionary<string, object> propertiesValues =
                PropertyLinkerHandler.GetPropertiesValues(checkboxId, isCheckboxChecked);

            if (propertiesValues.Count == 0)
            {
                throw new ValidationException($"{DateTime.Now}: Signature process is not possible. " +
                                              $"Signature data is not affected.", "");
            }
            #endregion

            #region Get form from database and check signature possibility
            Form statesAndSignatures = _formRepository.GetIsFreezedAndSignatures(formId);

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForObjectives &&
               !FormDataHandler.IsObjectivesSignaturePossible(statesAndSignatures))
            {
                throw new ValidationException($"{DateTime.Now}: Signature process is not possible. " +
                                              $"Objectives should be freezed at first.", "");
            }

            if (PropertyLinkerHandler.AffectedPropertyLinker.PropertyType == PropertyType.ForResults &&
               !FormDataHandler.IsResultsSignaturePossible(statesAndSignatures))
            {
                throw new ValidationException($"{DateTime.Now}: Signature process is not possible. " +
                                              $"Results should be freezed at first.", "");
            }
            #endregion

            #region Fill property-value pair with User signature and Update Form data
            FormDataHandler.PutUserSignature(ref propertiesValues);
            FormDataHandler.UpdateSignatures(statesAndSignatures, propertiesValues);
            FormDataHandler.UpdateLastSavedFormData(statesAndSignatures);
            _formRepository.UpdateFormSignatures(statesAndSignatures);
            #endregion

            return propertiesValues;
        }

        public FormDTO UpdateForm(DefinitionDTO definition,
                                  ConclusionDTO conclusion,
                                  SignaturesDTO signatures,
                                  IList<ObjectiveResultDTO> objectivesResultsDTO)
        {


            return new FormDTO();
        }
    }
}
