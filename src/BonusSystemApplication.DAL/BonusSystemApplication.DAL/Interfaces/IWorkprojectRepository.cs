using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IWorkprojectRepository
    {
        Task<Workproject> GetWorkprojectDataAsync(long workprojectId);
        Task<List<Workproject>> GetWorkprojectsNamesAsync();
        Task<bool> IsWorkprojectExistsAsync(long workprojectId);
    }
}
