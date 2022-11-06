using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public class WorkprojectRepository :IWorkprojectRepository
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
    }
}
