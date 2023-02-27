using BonusSystemApplication.DAL.Interfaces;
using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.DAL.EF;

namespace BonusSystemApplication.DAL.Repositories
{
    public class WorkprojectRepository : IWorkprojectRepository
    {
        private DataContext context;
        public WorkprojectRepository(DataContext ctx) => context = ctx;

        public Workproject GetWorkprojectData(long workprojectId)
        {
            return context.Workprojects
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
            return context.Workprojects
                .Select(w => new Workproject
                {
                    Id = w.Id,
                    Name= w.Name,
                })
                .ToList();
        }
    }
}
