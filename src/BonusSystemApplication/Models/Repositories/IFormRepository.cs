﻿namespace BonusSystemApplication.Models.Repositories
{
    public interface IFormRepository
    {
        IEnumerable<Form> GetForms();
        Form GetFormData(long formId);
        Form GetIsFreezedAndSignatureData(long formId);
        Form GetObjectivesResultsData(long formId);


        IQueryable<Form> GetAllFormsQuery(IEnumerable<GlobalAccess> globalAccesses, long userId);
        IQueryable<Form> GetFormsWithGlobalAccess(IEnumerable<GlobalAccess> globalAccesses);
        IQueryable<Form> GetFormsWithLocalAccess(long userId);
        IQueryable<Form> GetFormsWithParticipation(long userId);

        //IQueryable<Form> GetDefinition(long formId);
        //IQueryable<Form> GetObjectives(long formId);
        //IQueryable<Form> GetResults(long formId);
        //IQueryable<Form> GetConclusion(long formId);

        void CreateForm(Form form);
        void UpdateFormSignatures(Form form);
        void UpdateFormObjectivesResults(Form form);
        void DeleteForm(long id);
    }
}