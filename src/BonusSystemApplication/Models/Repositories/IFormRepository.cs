namespace BonusSystemApplication.Models.Repositories
{
    public interface IFormRepository
    {
        List<Form> GetForms(List<long> formIds);
        Form GetForm(long formId);


        Definition GetDefinition(long formId);
        IList<ObjectiveResult> GetObjectivesResults(long formId);
        IList<ObjectiveResult> GetObjectives(long formId);
        IList<ObjectiveResult> GetResults(long formId);
        Conclusion GetConclustion(long formId);



        Form GetIsFreezedAndSignatures(long formId);
        Form GetObjectivesResultsData(long formId);

        List<long> GetFormIdsWhereLocalAccess(long userId);


        void CreateForm(Form form);
        void UpdateFormSignatures(Form form);
        void UpdateFormObjectivesResults(Form form);
        void DeleteForm(long id);

    }
}