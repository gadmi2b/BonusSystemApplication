using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class UserDataSinglton
    {
        private static bool isCreated = false;
        public long UserId { get; }
        public IEnumerable<FormGlobalAccess> FormGlobalAccesses { get; }

        public UserDataSinglton(long userId, IEnumerable<FormGlobalAccess> formGlobalAccesses)
        {
            if (isCreated)
            {
                throw new Exception("Only one instance of object is allowed to be created");
            }

            isCreated = true;
            UserId = userId;
            FormGlobalAccesses = formGlobalAccesses;
        }

        /// <summary>
        /// Determine which accesses incoming from has and collect them into out List
        /// </summary>
        /// <param name="form">Incoming form to check</param>
        /// <param name="accessFilters">Out for collecting accesses</param>
        /// <returns>true if at least one access was found</returns>
        public bool GetPermissions(Form form, out List<Permissions> permissions)
        {
            List<Permissions> permissionsTemp = new List<Permissions>();

            Func<Form, bool> delegateAccess;

            foreach (var formGA in FormGlobalAccesses)
            {
                delegateAccess = ExpressionBuilder.GetExpressionForGlobalAccess(formGA).Compile();
                if (delegateAccess.Invoke(form))
                {
                    permissionsTemp.Add(Permissions.GlobalAccess);
                }
            }

            delegateAccess = ExpressionBuilder.GetExpressionForLocalAccess(UserId).Compile();
            if (delegateAccess.Invoke(form))
            {
                permissionsTemp.Add(Permissions.LocalAccess);
            }

            delegateAccess = ExpressionBuilder.GetMethodForParticipation(UserId, Permissions.Employee);
            if (delegateAccess.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Employee);
            }

            delegateAccess = ExpressionBuilder.GetMethodForParticipation(UserId, Permissions.Manager);
            if (delegateAccess.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Manager);
            }

            delegateAccess = ExpressionBuilder.GetMethodForParticipation(UserId, Permissions.Approver);
            if (delegateAccess.Invoke(form))
            {
                permissionsTemp.Add(Permissions.Approver);
            }

            permissions = permissionsTemp;

            if (permissionsTemp.Count > 0) return true;
            else return false;
        }
    }
}
