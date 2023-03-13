﻿using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.DAL.Interfaces
{
    public interface IDefinitionRepository
    {
        List<long> GetFormIdsWhereParticipation(long userId);
        List<long> GetFormIdsWhereGlobalAccess(IEnumerable<GlobalAccess> globalAccesses);

        Definition GetDefinition(long formId);
        Definition GetDefinitionFull(long formId);
        bool IsExistWithSamePropertyCombination(Definition definition, long formId);
    }
}
