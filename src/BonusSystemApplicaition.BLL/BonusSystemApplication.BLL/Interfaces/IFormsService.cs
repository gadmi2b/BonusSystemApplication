using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.BLL.Interfaces
{
    public interface IFormsService
    {
        FormIndexDTO GetFormIndexDTO(UserSelectionsDTO userSelections);
        FormDTO GetFormDTO(long formId);
        FormDTO GetIsFreezedStates(long formId);

        DefinitionDTO GetDefinitionDTO(long formId);
        ConclusionDTO GetConclusionDTO(long formId);
        SignaturesDTO GetSignaturesDTO(long formId);
        IList<ObjectiveResultDTO> GetObjectivesResultsDTO(long formId);

        Dictionary<string, string> GetUsersNames();
        Dictionary<string, string> GetPeriodsNames();
        Dictionary<string, string> GetWorkprojectsNames();

        void UpdateForm(long formId,
                        DefinitionDTO definition,
                        ConclusionDTO conclusion,
                        SignaturesDTO signatures,
                        IList<ObjectiveResultDTO> objectivesResultsDTO);
        void CreateForm(DefinitionDTO definition,
                        IList<ObjectiveResultDTO> objectivesResultsDTO);
        void ChangeState(string act,
                         string type,
                         long formId);

        string GetWorkprojectDescription(long workprojectId);
        EmployeeDTO GetEmployeeDTO(long userId);

        Dictionary<string, object> UpdateAndReturnSignatures(long formId,
                                                             string checkboxId,
                                                             bool isCheckboxChecked);
    }
}
