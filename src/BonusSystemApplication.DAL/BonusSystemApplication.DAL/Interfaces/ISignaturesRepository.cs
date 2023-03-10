using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface ISignaturesRepository
    {
        Signatures GetSignatures(long formId);
        void DropSignatures(long formId);
        void DropSignaturesForResults(long formId);
    }
}
