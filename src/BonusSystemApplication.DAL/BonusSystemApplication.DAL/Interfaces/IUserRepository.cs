using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserDataAsync(long userId);
        Task<List<User>> GetUsersNamesAsync();
        Task<bool> IsUserExistAsync(long userId);
    }
}
