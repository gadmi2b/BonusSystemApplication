using BonusSystemApplication.Models.Repositories;
using System.Runtime.CompilerServices;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class FormDataSingleton
    {
        private static bool isCreated = false;
        private List<Form> AvailableForms { get; }
        private IEnumerable<FormGlobalAccess> FormGlobalAccesses { get; }

        public List<Permissions> AvailablePermissions { get; set; }
        public List<string> AvailableEmployees { get; set; }
        public List<Periods> AvailablePeriods { get; set; }
        public List<int> AvailableYears { get; set; }
        public List<string> AvailableDepartments { get; set; }
        public List<string> AvailableTeams { get; set; }
        public List<string> AvailableWorkprojects { get; set; }

        public FormDataSingleton(List<Form> availableForms, IEnumerable<FormGlobalAccess> formGlobalAccesses)
        {
            if (isCreated)
            {
                throw new Exception("Only one instance is allowed to be created");
            }
            isCreated = true;

            AvailableForms = availableForms;
            FormGlobalAccesses = formGlobalAccesses;

            AvailablePermissions = SortFormsByPermissions();

            AvailableEmployees = GetAvailableEmployees();
            AvailablePeriods = GetAvailablePeriods();
            AvailableYears = GetAvailableYears();
            AvailableDepartments = GetAvailableDepartments();
            AvailableTeams = GetAvailableTeams();
            AvailableWorkprojects = GetAvailableWorkprojects();
        }

        private List<Permissions> SortFormsByPermissions()
        {
            long UserId = UserData.UserId;
            List<long> formIdsWithGlobalAccess = GetFormIdsWithGlobalAccess();
            List<long> formIdsWithLocalAccess = GetFormIdsWithLocalAccess(UserId);
            List<long> formIdsWithEmployeeParticipation = GetFormIdsWithEmployeeParticipation(UserId);
            List<long> formIdsWithManagerParticipation = GetFormIdsWithManagerParticipation(UserId);
            List<long> formIdsWithApproverParticipation = GetFormIdsWithApproverParticipation(UserId);

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
        private List<long> GetFormIdsWithGlobalAccess()
        {
            List<long> formIdsWithGlobalAccess = new List<long>();

            foreach (FormGlobalAccess formGA in FormGlobalAccesses)
            {
                Func<Form, bool> delegateGA = ExpressionBuilder.GetExpressionForGlobalAccess(formGA).Compile();
                formIdsWithGlobalAccess = AvailableForms
                    .Where(f => delegateGA.Invoke(f))
                    .Select(f => f.Id)
                    .ToList();
            }

            return formIdsWithGlobalAccess;
        }
        private List<long> GetFormIdsWithLocalAccess(long userId)
        {
            Func<Form, bool> delegateLA = ExpressionBuilder.GetExpressionForLocalAccess(userId).Compile();
            List<long> formIdsWithLocalAccess = AvailableForms
                .Where(f => delegateLA.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithLocalAccess;
        }
        private List<long> GetFormIdsWithEmployeeParticipation(long userId)
        {
            Func<Form, bool> delEmployeeParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Employee);
            List<long> formIdsWithEmployeeParticipation = AvailableForms
                .Where(f => delEmployeeParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithEmployeeParticipation;
        }
        private List<long> GetFormIdsWithManagerParticipation(long userId)
        {
            Func<Form, bool> delManagerParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Manager);
            List<long> formIdsWithManagerParticipation = AvailableForms
                .Where(f => delManagerParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithManagerParticipation;
        }
        private List<long> GetFormIdsWithApproverParticipation(long userId)
        {
            Func<Form, bool> delApproverParticipation = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Approver);
            List<long> formIdsWithApproverParticipation = AvailableForms
                .Where(f => delApproverParticipation.Invoke(f))
                .Select(f => f.Id)
                .ToList();

            return formIdsWithApproverParticipation;
        }

        private List<string> GetAvailableEmployees()
        {
            List<string> availableEmployees = new List<string>();
            availableEmployees = AvailableForms
                .Select(f => ($"{f.Employee.LastNameEng} {f.Employee.FirstNameEng}"))
                .Distinct()
                .ToList();

            return availableEmployees;
        }
        private List<Periods> GetAvailablePeriods()
        {
            List<Periods> availablePeriods = new List<Periods>();
            availablePeriods = AvailableForms
                .Select(f => f.Period)
                .Distinct()
                .ToList();

            return availablePeriods;
        }
        private List<int> GetAvailableYears()
        {
            List<int> availableYears = new List<int>();
            availableYears = AvailableForms
                .Select(f => f.Year)
                .Distinct()
                .ToList();

            return availableYears;
        }
        private List<string> GetAvailableDepartments()
        {
            List<string> availableDepartments = new List<string>();
            availableDepartments = AvailableForms
                .Select(f => f.Employee.Department.Name)
                .Distinct()
                .ToList();

            return availableDepartments;
        }
        private List<string> GetAvailableTeams()
        {
            List<string> availableTeams = new List<string>();
            availableTeams = AvailableForms
                .Select(f => f.Employee.Team.Name)
                .Distinct()
                .ToList();

            return availableTeams;
        }
        private List<string> GetAvailableWorkprojects()
        {
            List<string> availableWorkprojects = new List<string>();
            availableWorkprojects = AvailableForms
                .Select(f => f.Workproject.Name)
                .Distinct()
                .ToList();

            return availableWorkprojects;
        }


        /// <summary>
        /// Determine which permissions incoming from has and collect them into out List
        /// </summary>
        /// <param name="form">Incoming form to check</param>
        /// <param name="permissionNames">Out for collecting permissions</param>
        /// <returns>true if at least one permission was found</returns>
        public bool IsFormHasPermissions(Form form, out List<string> permissionNames)
        {
            long userId = UserData.UserId;

            List<Permissions> permissionsTemp = new List<Permissions>();
            Func<Form, bool> delegatePermission;

            foreach (var formGA in FormGlobalAccesses)
            {
                delegatePermission = ExpressionBuilder.GetExpressionForGlobalAccess(formGA).Compile();
                if (delegatePermission.Invoke(form))
                {
                    permissionsTemp.Add(Permissions.GlobalAccess);
                }
            }

            delegatePermission = ExpressionBuilder.GetExpressionForLocalAccess(userId).Compile();
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.LocalAccess);
            }

            delegatePermission = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Employee);
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Employee);
            }

            delegatePermission = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Manager);
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Manager);
            }

            delegatePermission = ExpressionBuilder.GetMethodForParticipation(userId, Permissions.Approver);
            if (delegatePermission.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Approver);
            }

            List<string> permissionNamesTemp = new List<string>();
            foreach (Permissions p in permissionsTemp)
            {
                permissionNamesTemp.Add(p.ToString());
            }
            permissionNames = permissionNamesTemp;

            if (permissionNamesTemp.Count > 0) return true;
            else return false;
        }
    }
}
