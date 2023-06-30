using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;

namespace BonusSystemApplication.BLL.Interfaces
{
    public interface IFormsService
    {
        Task<FormIndexDTO> GetFormIndexDtoAsync(UserSelectionsDTO userSelections);
        Task<FormDTO> GetFormDtoAsync(long formId);
        Task<FormDTO> GetPrefilledFormDtoAsync(long formId);
        Task<FormDTO> GetIsFrozenStatesAsync(long formId);
        Task<EmployeeDTO> GetEmployeeDtoAsync(long userId);
        Task<SignaturesDTO> GetSignaturesDtoAsync(long formId);
        Task<string> GetWorkprojectDescriptionAsync(long workprojectId);
        Task<Dictionary<string, string>> GetWorkprojectIdsNamesAsync();
        Task<Dictionary<string, string>> GetUserIdsNamesAsync();
        List<string> GetPeriodNames();

        Task<Dictionary<string, object>> UpdateAndReturnSignaturesAsync(long formId,
                                                             string checkboxId,
                                                             bool isCheckboxChecked);
        Task UpdateFormAsync(long formId,
                        DefinitionDTO definition,
                        ConclusionDTO conclusion,
                        List<ObjectiveResultDTO> objectivesResultsDTO);

        Task CreateFormAsync(DefinitionDTO definition,
                        ConclusionDTO conclusion,
                        List<ObjectiveResultDTO> objectivesResultsDTO);

        Task ChangeStateAsync(long formId,
                         string changeToState,
                         string objectivesOrResults);

        Task<string> DeleteFormAsync(long formId);

        Task<string> PromoteFormAsync(long formId);
    }
}
