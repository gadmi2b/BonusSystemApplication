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

        public Workproject GetWorkprojectData(long workprojectId)
        {
            return _context.Workprojects
                .AsNoTracking()
                .Where(w => w.Id == workprojectId)
                .Select(w => new Workproject
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description == null ? string.Empty : w.Description,
                })
                .First();
        }
        public List<Workproject> GetWorkprojectsNames()
        {
            return _context.Workprojects
                .AsNoTracking()
                .Select(w => new Workproject
                {
                    Id = w.Id,
                    Name= w.Name,
                })
                .ToList();
        }
        public bool IsWorkprojectExists(long workprojectId)
        {
            return _context.Workprojects.Any(w => w.Id == workprojectId);
        }
    }
}
