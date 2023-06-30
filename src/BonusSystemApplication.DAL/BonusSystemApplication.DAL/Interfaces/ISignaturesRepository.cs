using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface ISignaturesRepository
    {
        Task<Signatures> GetSignaturesAsync(long formId);
        Task DropSignaturesAsync(long formId);
        Task DropSignaturesForResultsAsync(long formId);
    }
}
