using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IObjectiveResultRepository
    {
        List<ObjectiveResult> GetObjectivesResults(long formId);
    }
}
