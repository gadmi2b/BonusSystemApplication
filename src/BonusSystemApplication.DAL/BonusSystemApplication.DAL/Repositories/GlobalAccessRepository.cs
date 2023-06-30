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

        public async Task<IEnumerable<GlobalAccess>> GetGlobalAccessesByUserIdAsync(long userId)
        {
            return await _context.GlobalAccess.AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
    }
}
