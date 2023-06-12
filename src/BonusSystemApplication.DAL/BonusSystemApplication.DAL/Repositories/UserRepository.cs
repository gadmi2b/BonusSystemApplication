using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext ctx) => _context = ctx;

        public User GetUserData(long userId)
        {
            return _context.Users
                .AsNoTracking()
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
            return _context.Users
                .AsNoTracking()
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
            return _context.Users.Any(u => u.Id == userId);
        }
    }
}
