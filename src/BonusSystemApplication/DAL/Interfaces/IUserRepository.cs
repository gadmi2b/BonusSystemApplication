using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IUserRepository
    {
        User GetUserData(long userId);
    }
}
