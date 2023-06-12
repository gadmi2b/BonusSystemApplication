using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IFormRepository
    {
        Form GetForm(long formId);
        Form GetFormForPromoting(long formId);
        List<Form> GetForms(List<long> formIds);
        Form GetStates(long formId);
        Form GetStatesAndSignatures(long formId);
        List<long> GetFormIdsWhereLocalAccess(long userId);

        void CreateForm(Form form);

        void UpdateStates(Form form);
        void UpdateSignatures(Form form);
        void UpdateResultsConclusion(Form form);
        void UpdateConclusionComments(Form form);
        void UpdateDefinitionObjectivesResultsConclusion(Form form);

        void DeleteForm(long formId);

    }
}