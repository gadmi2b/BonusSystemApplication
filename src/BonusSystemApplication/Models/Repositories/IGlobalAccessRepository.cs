namespace BonusSystemApplication.Models.Repositories
{
    public interface IGlobalAccessRepository
    {
        IEnumerable<GlobalAccess> GetGlobalAccessesByUserId(long userId);
    }
}
