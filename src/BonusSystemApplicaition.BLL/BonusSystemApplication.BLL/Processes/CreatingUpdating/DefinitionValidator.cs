using AutoMapper;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.BLL.Processes.CreatingUpdating
{
    internal class DefinitionValidator
    {
        private IMapper _mapper;
        private IUserRepository _userRepository;
        private IDefinitionRepository _definitionRepository;
        private IWorkprojectRepository _workprojectRepository;
        private DefinitionDTO _definition;

        public DefinitionValidator(IMapper mapper,
                                   IUserRepository userRepository,
                                   IDefinitionRepository definitionRepository,
                                   IWorkprojectRepository workprojectRepository,
                                   DefinitionDTO definition)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _definitionRepository = definitionRepository;
            _workprojectRepository = workprojectRepository;
            _definition = definition;
        }

        public void ValidateInputPresence()
        {
            if (_definition.EmployeeId <= 0)
            {
                throw new ValidationException("The form could't be saved without selected Employee.", "EmployeeId");
            }
            if (!_userRepository.IsUserExists(_definition.EmployeeId))
            {
                throw new ValidationException("Selected Employee is not registered in the system.", "EmployeeId");
            }

            if (_definition.WorkprojectId == 0)
            {
                throw new ValidationException("The form could't be saved without selected Workproject", "WorkprojectId");
            }
            if (!_workprojectRepository.IsWorkprojectExists(_definition.WorkprojectId))
            {
                throw new ValidationException("Selected Workproject is not registered in the system.", "WorkprojectId");
            }

            if (_definition.Year == 0)
            {
                throw new ValidationException("The form could't be saved without selected Year", "Year");
            }
            if (_definition.Year < DateTime.Now.Year - 1 ||
                _definition.Year > DateTime.Now.Year + 1)
            {
                throw new ValidationException("It's forbidden to save forms with more than '\u00B1'1 " +
                                              "year in the past or future.", "Year"); // +- sign
            }

            if (string.IsNullOrEmpty(_definition.Period))
            {
                throw new ValidationException("The form could't be saved without selected Period", "Period");
            }
            if (!Enum.TryParse(_definition.Period, out Periods period))
            {
                throw new ValidationException("Selected period is not allowed.", "Period");
            }
        }
        public void ValidateInputForChangeState()
        {
            if (_definition.ManagerId <= 0)
            {
                throw new ValidationException("Unable to change state of form without selected Manager.", "ManagerId");
            }
            if (!_userRepository.IsUserExists(_definition.ManagerId))
            {
                throw new ValidationException("Selected Manager is not registered in the system.", "ManagerId");
            }

            if (_definition.ApproverId <= 0)
            {
                throw new ValidationException("Unable to change state of form without selected Approver.", "ApproverId");
            }
            if (!_userRepository.IsUserExists(_definition.ApproverId))
            {
                throw new ValidationException("Selected Approver is not registered in the system.", "ApproverId");
            }
        }

        public void ValidateInputCombinationForCreate()
        {
            if (IsDefinitionExists())
            {
                throw new ValidationException("A form with selected employee, workproject, period and year is already exist. " +
                                              "Unable to create new form.", "");
            }
        }
        public void ValidateInputCombinationForUpdate()
        {
            // TODO: get Defintion by Id from definition repo
            //       if Inputs are different => IsDefinitionExists

            if (IsDefinitionExists())
            {
                throw new ValidationException("A form with selected employee, workproject, period and year is already exist. " +
                                              "Unable to update new form.", "");
            }
        }

        private bool IsDefinitionExists()
        {
            return _definitionRepository.IsDefinitionExists(_mapper.Map<Definition>(_definition));
        }
    }
}
