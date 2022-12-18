namespace BonusSystemApplication.Models.Repositories
{
    public interface IFormRepository
    {
        List<Form> GetForms(List<long> formIds);
        Form GetFormData(long formId);
        Form GetIsFreezedAndSignatureData(long formId);
        Form GetObjectivesResultsData(long formId);

        List<long> GetLocalAccessFormIds(long userId);

        //IQueryable<Form> GetDefinition(long formId);
        //IQueryable<Form> GetObjectives(long formId);
        //IQueryable<Form> GetResults(long formId);
        //IQueryable<Form> GetConclusion(long formId);

        void CreateForm(Form form);
        void UpdateFormSignatures(Form form);
        void UpdateFormObjectivesResults(Form form);
        void DeleteForm(long id);
    }
}