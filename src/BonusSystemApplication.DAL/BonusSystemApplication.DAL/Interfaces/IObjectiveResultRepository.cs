using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IObjectiveResultRepository
    {
        Task<List<ObjectiveResult>> GetObjectivesResultsAsync(long formId);
    }
}
