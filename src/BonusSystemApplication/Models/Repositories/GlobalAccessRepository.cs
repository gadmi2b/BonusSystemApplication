using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.Models.Repositories
{
    public class GlobalAccessRepository : IGlobalAccessRepository
    {
        private DataContext context;
        public GlobalAccessRepository(DataContext ctx) => context = ctx;

        public IEnumerable<GlobalAccess> GetGlobalAccessesByUserId(long userId)
        {
            return context.GlobalAccess
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToList();
        }
    }
}
