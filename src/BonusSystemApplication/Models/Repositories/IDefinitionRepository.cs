using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public interface IDefinitionRepository
    {
        List<long> GetFormIdsWhereParticipation(long userId);
        List<long> GetFormIdsWhereGlobalAccess(IEnumerable<GlobalAccess> globalAccesses);

    }
}
