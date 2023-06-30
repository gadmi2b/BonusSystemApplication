using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IFormRepository
    {
        Task<Form> GetFormAsync(long formId);
        Task<Form> GetFormForPromotingAsync(long formId);
        Task<List<Form>> GetFormsAsync(List<long> formIds);
        Task<Form> GetStatesAsync(long formId);
        Task<Form> GetStatesAndSignaturesAsync(long formId);
        Task<List<long>> GetFormIdsWhereLocalAccessAsync(long userId);

        Task CreateFormAsync(Form form);

        Task UpdateStatesAsync(Form form);
        Task UpdateSignaturesAsync(Form form);
        Task UpdateResultsConclusionAsync(Form form);
        Task UpdateConclusionCommentsAsync(Form form);
        Task UpdateDefinitionObjectivesResultsConclusionAsync(Form form);

        Task DeleteFormAsync(long formId);

    }
}