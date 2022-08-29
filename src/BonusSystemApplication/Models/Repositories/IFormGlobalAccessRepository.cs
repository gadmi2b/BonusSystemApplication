namespace BonusSystemApplication.Models.Repositories
{
    public interface IFormGlobalAccessRepository
    {
        IEnumerable<FormGlobalAccess> GetFormGlobalAccessByUserId(long userId);
    }
}
