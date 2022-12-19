using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public static class FormDataExtractor
    {

        public static List<Permission> GetPermissions(Form f,
                                                long userId,
                                                IEnumerable<long> gAccessFormIds,
                                                IEnumerable<long> lAccessFormIds,
                                                IEnumerable<long> participationFormIds)
        {
            List<Permission> permissions = new List<Permission>();
            if (gAccessFormIds.Contains(f.Id))
            {
                permissions.Add(Permission.GlobalAccess);
            }
            if (lAccessFormIds.Contains(f.Id))
            {
                permissions.Add(Permission.LocalAccess);
            }
            if (participationFormIds.Contains(f.Id))
            {
                if (userId == f.Definition.EmployeeId) { permissions.Add(Permission.Employee); }
                if (userId == f.Definition.ManagerId) { permissions.Add(Permission.Manager); }
                if (userId == f.Definition.ApproverId) { permissions.Add(Permission.Approver); }
            }

            return permissions;
        }
        public static List<Permission> GetAvailablePermissions(List<List<Permission>> allPermissions)
        {
            List<Permission> permissions = new List<Permission>();
            foreach (List<Permission> list in allPermissions)
            {
                foreach (Permission p in list)
                {
                    if (!permissions.Contains(p))
                    {
                        permissions.Add(p);
                    }
                }
            }
            return permissions;
        }
        public static List<string> GetAvailableEmployees(List<Form> forms)
        {
            List<string> availableEmployees = new List<string>();
            availableEmployees = forms
                .Select(f => ($"{f.Definition.Employee.LastNameEng} {f.Definition.Employee.FirstNameEng}"))
                .Distinct()
                .ToList();

            return availableEmployees;
        }
        public static List<Periods> GetAvailablePeriods(List<Form> forms)
        {
            List<Periods> availablePeriods = new List<Periods>();
            availablePeriods = forms
                .Select(f => f.Definition.Period)
                .Distinct()
                .ToList();

            return availablePeriods;
        }
        public static List<int> GetAvailableYears(List<Form> forms)
        {
            List<int> availableYears = new List<int>();
            availableYears = forms
                .Select(f => f.Definition.Year)
                .Distinct()
                .ToList();

            return availableYears;
        }
        public static List<string> GetAvailableDepartments(List<Form> forms)
        {
            List<string> availableDepartments = new List<string>();
            availableDepartments = forms
                .Select(f => f.Definition.Employee.Department.Name)
                .Distinct()
                .ToList();

            return availableDepartments;
        }
        public static List<string> GetAvailableTeams(List<Form> forms)
        {
            List<string> availableTeams = new List<string>();
            availableTeams = forms
                .Select(f => f.Definition.Employee.Team.Name)
                .Distinct()
                .ToList();

            return availableTeams;
        }
        public static List<string> GetAvailableWorkprojects(List<Form> forms)
        {
            List<string> availableWorkprojects = new List<string>();
            availableWorkprojects = forms
                .Select(f => f.Definition.Workproject.Name)
                .Distinct()
                .ToList();

            return availableWorkprojects;
        }
    }
}
