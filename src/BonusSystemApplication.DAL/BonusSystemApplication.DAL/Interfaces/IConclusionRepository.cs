using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IConclusionRepository
    {
        Conclusion GetConclusion(long formId);
    }
}
