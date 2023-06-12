using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;

namespace BonusSystemApplication.BLL.Interfaces
{
    public interface IFormsService
    {
        FormIndexDTO GetFormIndexDTO(UserSelectionsDTO userSelections);
        FormDTO GetFormDTO(long formId);
        FormDTO GetPrefilledFormDTO(long formId);
        FormDTO GetIsFrozenStates(long formId);
        EmployeeDTO GetEmployeeDTO(long userId);
        SignaturesDTO GetSignaturesDTO(long formId);
        string GetWorkprojectDescription(long workprojectId);
        Dictionary<string, string> GetWorkprojectIdsNames();
        Dictionary<string, string> GetUserIdsNames();
        List<string> GetPeriodNames();

        Dictionary<string, object> UpdateAndReturnSignatures(long formId,
                                                             string checkboxId,
                                                             bool isCheckboxChecked);
        void UpdateForm(long formId,
                        DefinitionDTO definition,
                        ConclusionDTO conclusion,
                        List<ObjectiveResultDTO> objectivesResultsDTO);
        
        void CreateForm(DefinitionDTO definition,
                        ConclusionDTO conclusion,
                        List<ObjectiveResultDTO> objectivesResultsDTO);
        
        void ChangeState(long formId,
                         string changeToState,
                         string objectivesOrResults);
        
        string DeleteForm(long formId);

        string PromoteForm(long formId);
    }
}
