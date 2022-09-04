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
        public bool GetAccessFilters(Form form, out List<AccessFilter> accessFilters)
        {
            List<AccessFilter> accesses = new List<AccessFilter>();

            Func<Form, bool> delegateAccess;

            foreach (var formGA in FormGlobalAccesses)
            {
                delegateAccess = ExpressionBuilder.GetExpressionForGlobalAccess(formGA).Compile();
                if (delegateAccess.Invoke(form))
                {
                    accesses.Add(AccessFilter.GlobalAccess);
                }

            }

            delegateAccess = ExpressionBuilder.GetExpressionForLocalAccess(UserId).Compile();
            if (delegateAccess.Invoke(form))
            {
                accesses.Add(AccessFilter.LocalAccess);
            }

            delegateAccess = ExpressionBuilder.GetMethodForParticipation(UserId, AccessFilter.Employee);
            if (delegateAccess.Invoke(form))
            {
                accesses.Add(AccessFilter.Employee);
            }

            delegateAccess = ExpressionBuilder.GetMethodForParticipation(UserId, AccessFilter.Manager);
            if (delegateAccess.Invoke(form))
            {
                accesses.Add(AccessFilter.Manager);
            }

            delegateAccess = ExpressionBuilder.GetMethodForParticipation(UserId, AccessFilter.Approver);
            if (delegateAccess.Invoke(form))
            {
                accesses.Add(AccessFilter.Approver);
            }

            accessFilters = accesses;

            if (accesses.Count > 0) return true;
            else return false;
        }
    }
}
