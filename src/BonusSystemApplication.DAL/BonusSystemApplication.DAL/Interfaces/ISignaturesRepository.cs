using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface ISignaturesRepository
    {
        Signatures GetSignatures(long formId);
    }
}
