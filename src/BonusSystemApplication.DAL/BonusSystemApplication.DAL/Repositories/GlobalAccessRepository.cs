using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class GlobalAccessRepository : IGlobalAccessRepository
    {
        private DataContext context;
        public GlobalAccessRepository(DataContext ctx) => context = ctx;

        public IEnumerable<GlobalAccess> GetGlobalAccessesByUserId(long userId)
        {
            return context.GlobalAccess.TagWith("Global accesses requesting")
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToList();
        }
    }
}
