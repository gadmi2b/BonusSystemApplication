using BonusSystemApplication.Models.Repositories;
using Microsoft.Data.SqlClient.Server;

namespace BonusSystemApplication.Models.BusinessLogic.SaveProcess
{
    public class SaveConfigurator
    {
        public List<SaveParts> Parts { get; set; } = new List<SaveParts>();

        public SaveConfigurator(Form statesAndSignatures)
        {
            bool isObjectiveSigned = statesAndSignatures.Signatures.ForObjectives.IsSignedByEmployee &&
                                     statesAndSignatures.Signatures.ForObjectives.IsSignedByManager &&
                                     statesAndSignatures.Signatures.ForObjectives.IsSignedByApprover;

            bool isResultsSigned = statesAndSignatures.Signatures.ForResults.IsSignedByEmployee &&
                                   statesAndSignatures.Signatures.ForResults.IsSignedByManager &&
                                   statesAndSignatures.Signatures.ForResults.IsSignedByApprover;

            if(isResultsSigned)
            {
                // Nothing could be saved
                Parts = null;
            }
            else if (statesAndSignatures.IsResultsFreezed)
            {
                // Conclusion could be saved
                Parts.Add(SaveParts.Conclusion);
            }
            else if (isObjectiveSigned)
            {
                // Results & Conclusion could be saved
                Parts.Add(SaveParts.Results);
                Parts.Add(SaveParts.Conclusion);
            }
            else if (statesAndSignatures.IsObjectivesFreezed)
            {
                // Results & Conclusion could be saved
                Parts.Add(SaveParts.Results);
                Parts.Add(SaveParts.Conclusion);
            }
            else
            {
                // Definition & Objectives & Results & Conclusion could be saved:
                // Definition & Objectives are take from form
                // Results: achieved is taken from form, others recalculated
                // Conclusion: Comments are taken from form, IsProposalForBonusPayment and OverallKPI recalculated
                Parts.Add(SaveParts.Definition);
                Parts.Add(SaveParts.Objectives);
                Parts.Add(SaveParts.Results);
                Parts.Add(SaveParts.Conclusion);
            }
        }
    
        public bool IsDataCouldBeUpdated(IFormRepository formRepo,
                                         Definition definition,
                                         List<ObjectiveResult> objectivesResults,
                                         Conclusion conclusion)
        {
            if (Parts.Contains(SaveParts.Definition))
            {
                // TODO: Is Definition possible to update?
                if(definition.EmployeeId == null ||
                   definition.Period == null ||
                   definition.Year == null ||
                   definition.WorkprojectId == null)
                {
                    return false;
                }

                // TODO: Is combination of these components is unique?
                
            }
            if (Parts.Contains(SaveParts.Objectives))
            {
                // TODO: Is Objectives possible to update?

            }
            if (Parts.Contains(SaveParts.Results))
            {
                // TODO: Is Results possible to update?

            }
            if (Parts.Contains(SaveParts.Conclusion))
            {
                // TODO: Is Conclusion possible to update?

            }

            return true;
        }
    }
}
