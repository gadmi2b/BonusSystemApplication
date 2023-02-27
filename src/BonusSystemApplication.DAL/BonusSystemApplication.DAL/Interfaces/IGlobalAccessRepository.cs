using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IGlobalAccessRepository
    {
        IEnumerable<GlobalAccess> GetGlobalAccessesByUserId(long userId);
    }
}
