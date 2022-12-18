using BonusSystemApplication.Models.ViewModels.Index;

namespace BonusSystemApplication.Models.ViewModels.IndexViewModel
{
    public static class DataHandler
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
    }
}
