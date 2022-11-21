using BonusSystemApplication.Models.ViewModels.FormViewModel;

namespace BonusSystemApplication.Models.Repositories
{
    public interface IFormRepository
    {
        Form GetForm(long id);
        IEnumerable<Form> GetForms();
        Form GetFormData(long formId);
        Form GetFormSignatureData(long formId);
        Form GetFormObjectivesResultsData(long formId);
        Form GetFormIsFreezedStates(long formId);
        IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<FormGlobalAccess> formGlobalAccesses);
        IQueryable<Form> GetFormsWithLocalAccess(long userId);
        IQueryable<Form> GetFormsWithParticipation(long userId);
        void CreateForm(Form form);
        void UpdateFormSignatures(Form form);
        void UpdateFormObjectivesResults(Form form);
        void DeleteForm(long id);
    }
}