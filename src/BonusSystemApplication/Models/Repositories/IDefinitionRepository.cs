using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public interface IDefinitionRepository
    {
        List<long> GetParticipationFormIds(long userId);
        List<long> GetGlobalAccessFormIds(IEnumerable<GlobalAccess> globalAccesses);

    }
}
