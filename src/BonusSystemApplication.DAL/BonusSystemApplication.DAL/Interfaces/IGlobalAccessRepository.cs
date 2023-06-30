using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IGlobalAccessRepository
    {
        Task<IEnumerable<GlobalAccess>> GetGlobalAccessesByUserIdAsync(long userId);
    }
}
