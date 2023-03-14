using AutoMapper;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.BLL.Processes
{
    internal class DefinitionValidator
    {
        private long _formId { get; set; } = 0;
        private IMapper _mapper { get; set; }
        private IUserRepository _userRepository { get; set; }
        private IDefinitionRepository _definitionRepository { get; set; }
        private IWorkprojectRepository _workprojectRepository { get; set; }
        private DefinitionDTO _definition { get; set; }

        public DefinitionValidator(long formId,
                                   IMapper mapper,
                                   IDefinitionRepository definitionRepository)
        {
            _formId = formId;
            _mapper = mapper;
            _definitionRepository = definitionRepository;

        }
        public DefinitionValidator(long formId,
                                   IMapper mapper,
                                   IUserRepository userRepository,
                                   IDefinitionRepository definitionRepository,
                                   IWorkprojectRepository workprojectRepository,
                                   DefinitionDTO definition)
        {
            _formId = formId;
            _mapper = mapper;
            _userRepository = userRepository;
            _definitionRepository = definitionRepository;
            _workprojectRepository = workprojectRepository;
            _definition = definition;
        }

        public void ValidateCreateProcess()
        {
            #region Mandatory properties are filled
            if (_definition.EmployeeId <= 0)
                throw new ValidationException("The form could't be saved without selected Employee.", "EmployeeId");

            if (_definition.WorkprojectId == 0)
                throw new ValidationException("The form could't be saved without selected Workproject", "WorkprojectId");

            if (_definition.Year == 0)
                throw new ValidationException("The form could't be saved without selected Year", "Year");

            if (string.IsNullOrEmpty(_definition.Period))
                throw new ValidationException("The form could't be saved without selected Period", "Period");
            #endregion

            #region Mandatory properties are permitted values
            if (!_userRepository.IsUserExists(_definition.EmployeeId))
                throw new ValidationException("Selected Employee is not registered in the system.", "EmployeeId");

            if (!_workprojectRepository.IsWorkprojectExists(_definition.WorkprojectId))
                throw new ValidationException("Selected Workproject is not registered in the system.", "WorkprojectId");

            if (_definition.Year < DateTime.Now.Year - 1 ||
                _definition.Year > DateTime.Now.Year + 1)
                throw new ValidationException("It's forbidden to save forms with more than '\u00B1'1 " +
                                              "year in the past or future.", "Year"); // +- sign

            if (!Enum.TryParse(_definition.Period, out Periods period))
                throw new ValidationException("Selected period is not allowed.", "Period");
            #endregion

            #region Mandatory property combination is unique
            if (!_definitionRepository.IsExistWithSamePropertyCombination(_mapper.Map<Definition>(_definition), _formId))
                throw new ValidationException("A form with selected employee, workproject, period and year is already exist. " +
                                              "Unable to create new form.", "");
            #endregion

            #region Additional properties are permitted values
            if (_definition.ManagerId > 0)
                if (!_userRepository.IsUserExists(_definition.ManagerId))
                    throw new ValidationException("Selected Manager is not registered in the system.", "ManagerId");

            if (_definition.ApproverId > 0)
                if (!_userRepository.IsUserExists(_definition.ApproverId))
                    throw new ValidationException("Selected Approver is not registered in the system.", "ApproverId");
            #endregion
        }

        public void ValidateUpdateProcess()
        {
            #region Mandatory properties are filled
            if (_definition.EmployeeId <= 0)
                throw new ValidationException("The form could't be saved without selected Employee.", "EmployeeId");

            if (_definition.WorkprojectId == 0)
                throw new ValidationException("The form could't be saved without selected Workproject", "WorkprojectId");

            if (_definition.Year == 0)
                throw new ValidationException("The form could't be saved without selected Year", "Year");

            if (string.IsNullOrEmpty(_definition.Period))
                throw new ValidationException("The form could't be saved without selected Period", "Period");
            #endregion

            #region Mandatory properties are permitted values
            if (!_userRepository.IsUserExists(_definition.EmployeeId))
                throw new ValidationException("Selected Employee is not registered in the system.", "EmployeeId");

            if (!_workprojectRepository.IsWorkprojectExists(_definition.WorkprojectId))
                throw new ValidationException("Selected Workproject is not registered in the system.", "WorkprojectId");

            if (_definition.Year < DateTime.Now.Year - 1 ||
                _definition.Year > DateTime.Now.Year + 1)
                throw new ValidationException("It's forbidden to save forms with more than '\u00B1'1 " +
                                              "year in the past or future.", "Year"); // +- sign

            if (!Enum.TryParse(_definition.Period, out Periods period))
                throw new ValidationException("Selected period is not allowed.", "Period");
            #endregion

            #region Mandatory property combination is unique
            if (!_definitionRepository.IsExistWithSamePropertyCombination(_mapper.Map<Definition>(_definition), _formId))
                throw new ValidationException("A form with selected employee, workproject, period and year is already exist. " +
                                              "Unable to update new form.", "");
            #endregion

            #region Additional properties are permitted values
            if (_definition.ManagerId > 0)
                if (!_userRepository.IsUserExists(_definition.ManagerId))
                    throw new ValidationException("Selected Manager is not registered in the system.", "ManagerId");

            if (_definition.ApproverId > 0)
                if (!_userRepository.IsUserExists(_definition.ApproverId))
                    throw new ValidationException("Selected Approver is not registered in the system.", "ApproverId");
            #endregion
        }

        public void ValidateChangeStateProcess()
        {
            DefinitionDTO originalDefinitionDTO = _mapper.Map<DefinitionDTO>(_definitionRepository.GetDefinition(_formId));

            #region Mandatory properties are filled
            if (originalDefinitionDTO.EmployeeId <= 0)
                throw new ValidationException("Unable to change state of form without selected Employee.", "EmployeeId");

            if (originalDefinitionDTO.WorkprojectId <= 0)
                throw new ValidationException("Unable to change state of form without selected Workproject", "WorkprojectId");

            if (originalDefinitionDTO.Year <= 0)
                throw new ValidationException("Unable to change state of form without selected Year", "Year");

            if (string.IsNullOrEmpty(originalDefinitionDTO.Period))
                throw new ValidationException("Unable to change state of form without selected Period", "Period");
            #endregion

            #region Additional properties are filled
            if (originalDefinitionDTO.ManagerId <= 0)
                throw new ValidationException("Unable to change state of form without selected Manger.", "ManagerId");

            if (originalDefinitionDTO.ApproverId <= 0)
                throw new ValidationException("Unable to change state of form without selected Approver", "ApproverId");
            #endregion
        }
    }
}
