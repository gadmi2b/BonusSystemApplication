using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.Models.Repositories
{
    public class FormGlobalAccessRepository : IFormGlobalAccessRepository
    {
        private DataContext context;
        public FormGlobalAccessRepository(DataContext ctx) => context = ctx;

        public IEnumerable<FormGlobalAccess> GetFormGlobalAccessByUserId(long userId)
        {
            return context.FormGlobalAccess
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToList();
        }
    }
}
