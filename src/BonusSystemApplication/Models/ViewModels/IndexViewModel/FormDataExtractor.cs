using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public static class FormDataExtractor
    {

        public static List<Permissions> GetPermissions(Form f,
                                                long userId,
                                                IEnumerable<long> gAccessFormIds,
                                                IEnumerable<long> lAccessFormIds,
                                                IEnumerable<long> participationFormIds)
        {
            List<Permissions> permissions = new List<Permissions>();
            if (gAccessFormIds.Contains(f.Id))
            {
                permissions.Add(Permissions.GlobalAccess);
            }
            if (lAccessFormIds.Contains(f.Id))
            {
                permissions.Add(Permissions.LocalAccess);
            }
            if (participationFormIds.Contains(f.Id))
            {
                if (userId == f.Definition.EmployeeId) { permissions.Add(Permissions.Employee); }
                if (userId == f.Definition.ManagerId) { permissions.Add(Permissions.Manager); }
                if (userId == f.Definition.ApproverId) { permissions.Add(Permissions.Approver); }
            }

            return permissions;
        }





        public List<string> GetAvailableEmployees(List<Form> forms)
        {
            List<string> availableEmployees = new List<string>();
            availableEmployees = forms
                .Select(f => ($"{f.Definition.Employee.LastNameEng} {f.Definition.Employee.FirstNameEng}"))
                .Distinct()
                .ToList();

            return availableEmployees;
        }
        public List<Periods> GetAvailablePeriods(List<Form> forms)
        {
            List<Periods> availablePeriods = new List<Periods>();
            availablePeriods = forms
                .Select(f => f.Definition.Period)
                .Distinct()
                .ToList();

            return availablePeriods;
        }
        public List<int> GetAvailableYears(List<Form> forms)
        {
            List<int> availableYears = new List<int>();
            availableYears = forms
                .Select(f => f.Definition.Year)
                .Distinct()
                .ToList();

            return availableYears;
        }
        public List<string> GetAvailableDepartments(List<Form> forms)
        {
            List<string> availableDepartments = new List<string>();
            availableDepartments = forms
                .Select(f => f.Definition.Employee.Department.Name)
                .Distinct()
                .ToList();

            return availableDepartments;
        }
        public List<string> GetAvailableTeams(List<Form> forms)
        {
            List<string> availableTeams = new List<string>();
            availableTeams = forms
                .Select(f => f.Definition.Employee.Team.Name)
                .Distinct()
                .ToList();

            return availableTeams;
        }
        public List<string> GetAvailableWorkprojects(List<Form> forms)
        {
            List<string> availableWorkprojects = new List<string>();
            availableWorkprojects = forms
                .Select(f => f.Definition.Workproject.Name)
                .Distinct()
                .ToList();

            return availableWorkprojects;
        }
        public List<Permissions> GetAvailablePermissions(List<Form> forms, long userId, IEnumerable<GlobalAccess> globalAccesses)
        {
            List<long> formIdsWithGlobalAccess = GetFormIdsWithGlobalAccess(forms, globalAccesses);
            List<long> formIdsWithLocalAccess = GetFormIdsWithLocalAccess(forms, userId);
            List<long> formIdsWithEmployeeParticipation = GetFormIdsWithEmployeeParticipation(forms, userId);
            List<long> formIdsWithManagerParticipation = GetFormIdsWithManagerParticipation(forms, userId);
            List<long> formIdsWithApproverParticipation = GetFormIdsWithApproverParticipation(forms, userId);

            List<Permissions> availablePermissions = new List<Permissions>();
            if (formIdsWithGlobalAccess.Count > 0)
            {
                availablePermissions.Add(Permissions.GlobalAccess);
            }
            if (formIdsWithLocalAccess.Count > 0)
            {
                availablePermissions.Add(Permissions.LocalAccess);
            }
            if (formIdsWithEmployeeParticipation.Count > 0)
            {
                availablePermissions.Add(Permissions.Employee);
            }
            if (formIdsWithManagerParticipation.Count > 0)
            {
                availablePermissions.Add(Permissions.Manager);
            }
            if (formIdsWithApproverParticipation.Count > 0)
            {
                availablePermissions.Add(Permissions.Approver);
            }

            return availablePermissions;
        }


        private List<long> GetFormIdsWithGlobalAccess(List<Form> forms, IEnumerable<GlobalAccess> globalAccesses)
        {
            List<long> formIdsWithGlobalAccess = new List<long>();

            foreach (GlobalAccess formGA in globalAccesses)
            {
                Func<Form, bool> delegateGA = ExpressionBuilder.GetExpressionForGlobalAccess(formGA).Compile();
                formIdsWithGlobalAccess = forms
                    .Where(f => delegateGA.Invoke(f))
                    .Select(f => f.Id)
                    .ToList();
            }

            return formIdsWithGlobalAccess;
        }
        private List<long> GetFormIdsWithLocalAccess(List<Form> forms, long userId)
        {
            Func<Form, bool> delegateLA = ExpressionBuilder.GetExpressionForLocalAccess(userId).Compile();
            List<long> formIdsWithLocalAccess = forms
                .Where(f => delegateLA.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithLocalAccess;
        }
        private List<long> GetFormIdsWithEmployeeParticipation(List<Form> forms, long userId)
        {
            Func<Form, bool> delEmployeeParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Employee);
            List<long> formIdsWithEmployeeParticipation = forms
                .Where(f => delEmployeeParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithEmployeeParticipation;
        }
        private List<long> GetFormIdsWithManagerParticipation(List<Form> forms, long userId)
        {
            Func<Form, bool> delManagerParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Manager);
            List<long> formIdsWithManagerParticipation = forms
                .Where(f => delManagerParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithManagerParticipation;
        }
        private List<long> GetFormIdsWithApproverParticipation(List<Form> forms, long userId)
        {
            Func<Form, bool> delApproverParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Approver);
            List<long> formIdsWithApproverParticipation = forms
                .Where(f => delApproverParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithApproverParticipation;
        }

    }
}
