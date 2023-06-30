using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IDefinitionRepository
    {
        Task<List<long>> GetFormIdsWhereParticipationAsync(long userId);
        Task<List<long>> GetFormIdsWhereGlobalAccessAsync(IEnumerable<GlobalAccess> globalAccesses);
        Task<Definition> GetDefinitionAsync(long formId);

        Task<bool> IsExistWithSamePropertyCombinationAsync(long formId,
                                                           long employeeId,
                                                           long workprojectId,
                                                           int year,
                                                           Periods period);
    }
}
