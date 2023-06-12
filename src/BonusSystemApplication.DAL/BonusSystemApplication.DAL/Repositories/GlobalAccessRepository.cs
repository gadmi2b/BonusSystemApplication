using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class GlobalAccessRepository : IGlobalAccessRepository
    {
        private DataContext _context;
        public GlobalAccessRepository(DataContext ctx) => _context = ctx;

        public IEnumerable<GlobalAccess> GetGlobalAccessesByUserId(long userId)
        {
            return _context.GlobalAccess
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToList();
        }
    }
}
