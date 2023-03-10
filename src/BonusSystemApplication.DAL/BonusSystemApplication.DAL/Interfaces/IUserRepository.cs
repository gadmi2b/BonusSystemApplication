using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IUserRepository
    {
        User GetUserData(long userId);
        List<User> GetUsersNames();
        bool IsUserExists(long userId);
    }
}
