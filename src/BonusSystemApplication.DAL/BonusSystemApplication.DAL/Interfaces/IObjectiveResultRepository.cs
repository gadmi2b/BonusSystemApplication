using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IObjectiveResultRepository
    {
        IList<ObjectiveResult> GetObjectivesResults(long formId);
    }
}
