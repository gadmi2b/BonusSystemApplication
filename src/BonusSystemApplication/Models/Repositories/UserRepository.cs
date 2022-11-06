using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public class UserRepository: IUserRepository
    {
        private DataContext context;
        public UserRepository(DataContext ctx) => context = ctx;

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
    }
}
