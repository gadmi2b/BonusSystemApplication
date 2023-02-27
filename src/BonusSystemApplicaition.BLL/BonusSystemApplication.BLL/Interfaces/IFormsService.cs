using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.DTO.Index;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.BLL.Interfaces
{
    public interface IFormsService
    {
        FormIndexDTO GetFormIndexDTO(UserSelectionsDTO userSelections);
        FormDTO GetFormDTO(long formId);

        DefinitionDTO GetDefinitionDTO(long formId);
        ConclusionDTO GetConclusionDTO(long formId);
        SignaturesDTO GetSignaturesDTO(long formId);
        IList<ObjectiveResultDTO> GetObjectivesResultsDTO(long formId);

        List<SelectListItem> GetUsersNames();
        List<SelectListItem> GetPeriodsNames();
        List<SelectListItem> GetWorkprojectsNames();

        FormDTO UpdateForm(DefinitionDTO definition,
                           ConclusionDTO conclusion,
                           SignaturesDTO signatures,
                           IList<ObjectiveResultDTO> objectivesResultsDTO);

        string GetWorkprojectDescription(long workprojectId);
        EmployeeDTO GetEmployeeDTO(long userId);
    }
}
