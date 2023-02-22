using BonusSystemApplication.DAL.Entities;
using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.Models.ViewModels.IndexViewModel
{
    public static class FormDataExtractor
    {

        public static List<Permission> ExtractPermissions(Form form,
                                                          long userId,
                                                          IEnumerable<long> formIdsWithGlobalAccess,
                                                          IEnumerable<long> formIdsWithLocalAccess,
                                                          IEnumerable<long> formIdsWithParticipation)
        {
            List<Permission> permissions = new List<Permission>();
            if (formIdsWithGlobalAccess.Contains(form.Id))
            {
                permissions.Add(Permission.GlobalAccess);
            }
            if (formIdsWithLocalAccess.Contains(form.Id))
            {
                permissions.Add(Permission.LocalAccess);
            }
            if (formIdsWithParticipation.Contains(form.Id))
            {
                if (userId == form.Definition.EmployeeId) { permissions.Add(Permission.Employee); }
                if (userId == form.Definition.ManagerId) { permissions.Add(Permission.Manager); }
                if (userId == form.Definition.ApproverId) { permissions.Add(Permission.Approver); }
            }
            return permissions;
        }

        public static List<Permission> ExtractPermissions(List<List<Permission>> allPermissions)
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

        public static List<string> ExtractEmployees(List<Form> forms)
        {
            List<string> employees = new List<string>();
            employees = forms
                .Select(f => $"{f.Definition.Employee.LastNameEng} {f.Definition.Employee.FirstNameEng}")
                .Distinct()
                .ToList();

            return employees;
        }

        public static List<Periods> ExtractPeriods(List<Form> forms)
        {
            List<Periods> periods = new List<Periods>();
            periods = forms
                .Select(f => f.Definition.Period)
                .Distinct()
                .ToList();

            return periods;
        }

        public static List<int> ExtractYears(List<Form> forms)
        {
            List<int> years = new List<int>();
            years = forms
                .Select(f => f.Definition.Year)
                .Distinct()
                .ToList();

            return years;
        }

        public static List<string> ExtractDepartments(List<Form> forms)
        {
            List<string> departments = new List<string>();
            departments = forms
                .Select(f => f.Definition.Employee.Department.Name)
                .Distinct()
                .ToList();

            return departments;
        }

        public static List<string> ExtractTeams(List<Form> forms)
        {
            List<string> teams = new List<string>();
            teams = forms
                .Select(f => f.Definition.Employee.Team.Name)
                .Distinct()
                .ToList();

            return teams;
        }

        public static List<string> ExtractWorkprojects(List<Form> forms)
        {
            List<string> workprojects = new List<string>();
            workprojects = forms
                .Select(f => f.Definition.Workproject.Name)
                .Distinct()
                .ToList();

            return workprojects;
        }
    }
}
