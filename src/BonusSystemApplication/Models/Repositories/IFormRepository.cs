using System.Linq.Expressions;
namespace BonusSystemApplication.Models.Repositories
{
    public interface IFormRepository
    {
        Form GetForm(long id);
        IEnumerable<Form> GetForms();
        IQueryable<Form> GetFormQuery(long formId);
        IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<FormGlobalAccess> formGlobalAccesses);
        IQueryable<Form> GetFormsWithLocalAccess(long userId);
        IQueryable<Form> GetFormsWithParticipation(long userId);
        void CreateForm(Form form);
        void UpdateForm(Form form);
        void DeleteForm(long id);
    }
}
