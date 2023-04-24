using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.BLL.Interfaces
{
    public interface IFormsService
    {
        FormIndexDTO GetFormIndexDTO(UserSelectionsDTO userSelections);
        FormDTO GetFormDTO(long formId);
        FormDTO GetIsFrozenStates(long formId);

        DefinitionDTO GetDefinitionDTO(long formId);
        ConclusionDTO GetConclusionDTO(long formId);
        SignaturesDTO GetSignaturesDTO(long formId);
        IList<ObjectiveResultDTO> GetObjectivesResultsDTO(long formId);

        Dictionary<string, string> GetWorkprojectIdsNames();
        Dictionary<string, string> GetUserIdsNames();
        List<string> GetPeriodNames();

        void UpdateForm(long formId,
                        DefinitionDTO definition,
                        ConclusionDTO conclusion,
                        List<ObjectiveResultDTO> objectivesResultsDTO);
        void CreateForm(DefinitionDTO definition,
                        ConclusionDTO conclusion,
                        List<ObjectiveResultDTO> objectivesResultsDTO);
        void ChangeState(long formId,
                         string act,
                         string type);

        string GetWorkprojectDescription(long workprojectId);
        EmployeeDTO GetEmployeeDTO(long userId);

        Dictionary<string, object> UpdateAndReturnSignatures(long formId,
                                                             string checkboxId,
                                                             bool isCheckboxChecked);
    }
}
