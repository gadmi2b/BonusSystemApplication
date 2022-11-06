namespace BonusSystemApplication.Models.Repositories
{
    public interface IUserRepository
    {
        User GetUserData(long userId);
    }
}
