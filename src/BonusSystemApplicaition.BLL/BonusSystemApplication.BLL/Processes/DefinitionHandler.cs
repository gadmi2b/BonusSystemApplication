using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.BLL.Processes
{
    internal class DefinitionHandler
    {
        private long _formId { get; set; } = 0;
        private IUserRepository _userRepository { get; set; }
        private IDefinitionRepository _definitionRepository { get; set; }
        private IWorkprojectRepository _workprojectRepository { get; set; }

        public DefinitionHandler() { }
        public DefinitionHandler(long formId,
                                 IUserRepository userRepository,
                                 IDefinitionRepository definitionRepository,
                                 IWorkprojectRepository workprojectRepository)
        {
            _formId = formId;
            _userRepository = userRepository;
            _definitionRepository = definitionRepository;
            _workprojectRepository = workprojectRepository;
        }

        /// <summary>
        /// Checks Definition incoming from Presentation layer
        /// during Create and Update form processes
        /// </summary>
        /// <param name="definitionDTO"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void HandleUpdateProcess(DefinitionDTO definitionDTO)
        {
            ArgumentNullException.ThrowIfNull(definitionDTO, nameof(definitionDTO));
            ArgumentNullException.ThrowIfNull(_userRepository, nameof(_userRepository));
            ArgumentNullException.ThrowIfNull(_definitionRepository, nameof(_definitionRepository));
            ArgumentNullException.ThrowIfNull(_workprojectRepository, nameof(_workprojectRepository));

            #region Mandatory properties are filled
            MustBeFilledAndGreaterThanZero(definitionDTO.WorkprojectId, "Workproject", nameof(definitionDTO.WorkprojectId));
            MustBeFilledAndGreaterThanZero(definitionDTO.EmployeeId, "Employee", nameof(definitionDTO.EmployeeId));
            MustBeFilledAndGreaterThanZero(definitionDTO.Year, "Year", nameof(definitionDTO.Year));
            MustBeFilled(definitionDTO.Period, "Period", nameof(definitionDTO.Period));
            #endregion

            #region Mandatory properties are permitted values
            UserMustExist(definitionDTO.EmployeeId, "Employee", nameof(definitionDTO.EmployeeId));
            WorkprojectMustExist(definitionDTO.WorkprojectId);
            YearMustBeLimited(definitionDTO.Year);

            if (!Enum.TryParse(definitionDTO.Period, out Periods period))
                throw new ValidationException($"Unable to perform operation. " +
                                              $"Selected period is not allowed.",
                                              $"{nameof(definitionDTO.Period)}");
            #endregion

            #region Mandatory property combination is unique
            if (_definitionRepository.IsExistWithSamePropertyCombination(_formId,
                                                                         definitionDTO.EmployeeId,
                                                                         definitionDTO.WorkprojectId,
                                                                         definitionDTO.Year,
                                                                         period))
                throw new ValidationException("Unable to perform operation. " +
                                              "Another form with selected employee, workproject, period and year is already exist.");
            #endregion

            #region Additional properties are permitted values
            if (definitionDTO.ManagerId > 0)
                UserMustExist(definitionDTO.ManagerId, "Manager", nameof(definitionDTO.ManagerId));

            if (definitionDTO.ApproverId > 0)
                UserMustExist(definitionDTO.ApproverId, "Approver", nameof(definitionDTO.ApproverId));
            #endregion
        }
        /// <summary>
        /// Checks Definition stored in Database
        /// during Objectives freezing process
        /// </summary>
        /// <param name="definition"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void HandleChangeStateProcess(Definition definition)
        {
            ArgumentNullException.ThrowIfNull(definition, nameof(definition));

            #region Mandatory properties are filled
            MustBeFilledAndGreaterThanZero(definition.WorkprojectId, "Workproject", nameof(definition.WorkprojectId));
            MustBeFilledAndGreaterThanZero(definition.EmployeeId, "Employee", nameof(definition.EmployeeId));
            MustBeFilledAndGreaterThanZero(definition.Year, "Year", nameof(definition.Year));
            #endregion

            #region Additional properties are filled
            MustBeFilledAndGreaterThanZero(definition.ApproverId, "Approver", nameof(definition.ApproverId));
            MustBeFilledAndGreaterThanZero(definition.ManagerId, "Manager", nameof(definition.ManagerId));
            #endregion
        }


        private void MustBeFilledAndGreaterThanZero(long? property, string selectedName, string propertyName)
        {
            if (property == null ||
                property <= 0)
                throw new ValidationException($"Unable to perform operation. " +
                                              $"{selectedName} must be selected.",
                                              $"{propertyName}");
        }
        private void MustBeFilled(string? property, string selectedName, string propertyName)
        {
            if (string.IsNullOrEmpty(property))
                throw new ValidationException($"Unable to perform operation. " +
                                              $"{selectedName} must be selected.",
                                              $"{propertyName}");
        }
        private void UserMustExist(long userId, string selectedName, string propertyName)
        {
            if (!_userRepository.IsUserExist(userId))
                throw new ValidationException($"Selected {selectedName} is not registered in the system.", $"{propertyName}");
        }
        private void WorkprojectMustExist(long workprojectId)
        {
            if (!_workprojectRepository.IsWorkprojectExists(workprojectId))
                throw new ValidationException("Selected Workproject is not registered in the system.",
                                              $"{nameof(DefinitionDTO.WorkprojectId)}");
        }
        private void YearMustBeLimited(int year)
        {
            if (year < DateTime.Now.Year - 1 ||
                year > DateTime.Now.Year + 1)
                throw new ValidationException("It's forbidden to save forms with more than \u00B11 " +  // \u00B1: +- sign
                                              "year in the past or future.",
                                              $"{nameof(DefinitionDTO.Year)}");
        }
    }
}
