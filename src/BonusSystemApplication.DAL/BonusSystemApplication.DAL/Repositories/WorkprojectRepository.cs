using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.DAL.Repositories
{
    public class WorkprojectRepository : IWorkprojectRepository
    {
        private DataContext _context;
        public WorkprojectRepository(DataContext ctx) => _context = ctx;

        public async Task<Workproject> GetWorkprojectDataAsync(long workprojectId)
        {
            return await _context.Workprojects.AsNoTracking()
                .Where(w => w.Id == workprojectId)
                .Select(w => new Workproject
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description == null ? string.Empty : w.Description,
                })
                .FirstAsync();
        }
        public async Task<List<Workproject>> GetWorkprojectsNamesAsync()
        {
            return await _context.Workprojects.AsNoTracking()
                .Select(w => new Workproject
                {
                    Id = w.Id,
                    Name= w.Name,
                })
                .ToListAsync();
        }
        public async Task<bool> IsWorkprojectExistsAsync(long workprojectId)
        {
            return await _context.Workprojects.AnyAsync(w => w.Id == workprojectId);
        }
    }
}
