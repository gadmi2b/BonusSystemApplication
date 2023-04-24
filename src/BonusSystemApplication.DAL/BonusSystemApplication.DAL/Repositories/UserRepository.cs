using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;

namespace BonusSystemApplication.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext context;
        public UserRepository(DataContext ctx)
        {
            Console.WriteLine($">>>>>>>>>> DI has created me at: {DateTime.Now} <<<<<<<<<<");
            context = ctx;
        }

        public User GetUserData(long userId)
        {
            return context.Users
                .Where(u => u.Id == userId)
                .Select(u => new User
                {
                    Id = u.Id,
                    Pid = u.Pid,
                    Position = new Position
                    {
                        Id = u.PositionId == null ? 0 : (long)u.PositionId,
                        NameEng = u.Position == null ? string.Empty : u.Position.NameEng,
                    },
                    Team = new Team
                    {
                        Id = u.TeamId == null ? 0 : (long)u.TeamId,
                        Name = u.Team == null ? string.Empty : u.Team.Name,
                    },
                })
                .First();
        }

        public List<User> GetUsersNames()
        {
            return context.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    FirstNameEng = u.FirstNameEng,
                    LastNameEng = u.LastNameEng,
                })
                .ToList();
        }
    
        public bool IsUserExist(long userId)
        {
            return context.Users.Any(u => u.Id == userId);
        }
    }
}
