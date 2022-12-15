using BonusSystemApplication.Models.Repositories;
using Microsoft.Data.SqlClient.Server;

namespace BonusSystemApplication.Models.BusinessLogic.SaveProcess
{
    public class SaveConfigurator
    {
        public List<UpdatableParts> updatableParts { get; set; } = new List<UpdatableParts>();

        // TODO: create a method returned List<IQueryable<Form>> GetFormDataQueries(formIsFreezedAndSignatures):
        //                                     IQueryable<Form> GetDefinition(formId)
        //                                     IQueryable<Form> GetObjectives(formId)
        //                                     IQueryable<Form> GetResults(formId)
        //                                     IQueryable<Form> GetConclusion(formId)
        //       and collect List<FormParts> partsToUpdate
        // TODO: create a method updated requested form depending on partsToUpdate and
        //       definition, objectivesResults, conclusion

        public SaveConfigurator(Form form)
        {
            bool isObjectiveSigned = form.Signatures.ForObjectives.IsSignedByEmployee &&
                                     form.Signatures.ForObjectives.IsSignedByManager &&
                                     form.Signatures.ForObjectives.IsSignedByApprover;

            bool isResultsSigned = form.Signatures.ForResults.IsSignedByEmployee &&
                                   form.Signatures.ForResults.IsSignedByManager &&
                                   form.Signatures.ForResults.IsSignedByApprover;

            if(isResultsSigned)
            {
                // TODO: nothing to save - exit
                updatableParts = null;
            }
            else if (form.IsResultsFreezed)
            {
                // TODO: only Conclusion could be saved
                updatableParts.Add(UpdatableParts.Conclusion);
            }
            else if (isObjectiveSigned)
            {
                // TODO: Results | Conclusion could be saved
                updatableParts.Add(UpdatableParts.Results);
                updatableParts.Add(UpdatableParts.Conclusion);
            }
            else if (form.IsObjectivesFreezed)
            {
                // TODO: Results | Conclusion could be saved
                updatableParts.Add(UpdatableParts.Results);
                updatableParts.Add(UpdatableParts.Conclusion);
            }
            else
            {
                // TODO: Definition | Objectives | Results | Conclusion could be saved
                updatableParts.Add(UpdatableParts.Definition);
                updatableParts.Add(UpdatableParts.Objectives);
                updatableParts.Add(UpdatableParts.Results);
                updatableParts.Add(UpdatableParts.Conclusion);
            }
        }
    
        public Form GetFormQuery(long formId, IFormRepository formRepo)
        {
            List<IQueryable<Form>> queries = new List<IQueryable<Form>>();
            foreach(UpdatableParts part in updatableParts)
            {
                if (part == UpdatableParts.Definition) { queries.Add(formRepo.GetDefinition(formId)); }
                if (part == UpdatableParts.Objectives) { queries.Add(formRepo.GetObjectives(formId)); }
                if (part == UpdatableParts.Results) { queries.Add(formRepo.GetResults(formId)); }
                if (part == UpdatableParts.Conclusion) { queries.Add(formRepo.GetConclusion(formId)); }
            }

            //queries.Add(formRepo.GetDefinition(formId));
            //queries.Add(formRepo.GetConclusion(formId));

            IQueryable<Form> combinedFormsQuery = null;
            for (int i = 0; i < queries.Count; i++)
            {
                if (i == 0)
                {
                    combinedFormsQuery = queries[i];
                }
                else
                {
                    combinedFormsQuery = combinedFormsQuery.Union(queries[i]);
                }
            }

            return combinedFormsQuery.ToList()[0]; // EF problem here
        }
    }
}
