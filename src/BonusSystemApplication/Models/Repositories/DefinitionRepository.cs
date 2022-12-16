using Microsoft.EntityFrameworkCore;

namespace BonusSystemApplication.Models.Repositories
{
    public class DefinitionRepository : IDefinitionRepository
    {
        private DataContext context;
        public DefinitionRepository(DataContext ctx) => context = ctx;

        public Form GetForm(long formId)
        {
            Form form = context.Definitions
                .Where(d => d.Id == formId)
                .Include(d => d.Form)
                    .ThenInclude(f => f.Definition)
                .Select(d => d.Form)
                .First();
            return form;
        }
    }
}
