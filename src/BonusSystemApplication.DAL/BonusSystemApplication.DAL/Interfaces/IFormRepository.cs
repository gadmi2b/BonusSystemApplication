using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IFormRepository
    {
        List<Form> GetForms(List<long> formIds);
        Form GetStates(long formId);
        void UpdateStates(Form form);


        Form GetForm(long formId);


        Definition GetDefinition(long formId);
        IList<ObjectiveResult> GetObjectivesResults(long formId);
        IList<ObjectiveResult> GetObjectives(long formId);
        IList<ObjectiveResult> GetResults(long formId);
        Conclusion GetConclustion(long formId);



        Form GetStatesAndSignatures(long formId);
        Form GetObjectivesResultsData(long formId);

        List<long> GetFormIdsWhereLocalAccess(long userId);


        void CreateForm(Form form);

        void UpdateSignatures(Form form);
        void UpdateResultsConclusion(Form form);
        void UpdateConclusionComments(Form form);
        void UpdateDefinitionObjectivesResultsConclusion(Form form);

        void DeleteForm(long id);

    }
}