using BonusSystemApplication.Models.ViewModels.Index;
using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<Definition, bool>> GetExpressionForGlobalAccess(GlobalAccess gAccess)
        {
            Expression<Func<Definition, bool>> expr = (Definition d) => false;

            if (gAccess.DepartmentId == null)
            {
                expr = (Definition d) => true;
            }
            else if (gAccess.TeamId == null)
            {
                expr = (Definition d) => d.Employee.DepartmentId == gAccess.DepartmentId;
            }
            else if (gAccess.WorkprojectId == null)
            {
                expr = (Definition d) => d.Employee.DepartmentId == gAccess.DepartmentId &&
                                         d.Employee.TeamId == gAccess.TeamId;
            }
            else
            {
                expr = (Definition d) => d.Employee.DepartmentId == gAccess.DepartmentId &&
                                         d.Employee.TeamId == gAccess.TeamId &&
                                         d.WorkprojectId == gAccess.WorkprojectId;
            }

            return expr;
        }
        public static Func<Form, bool> GetMethodForParticipation(long userId, Permissions participantRole)
        {
            if(participantRole == Permissions.Employee)
            {
                return (f) => f.Definition.EmployeeId == userId;
            }
            else if (participantRole == Permissions.Manager)
            {
                return (f) => f.Definition.ManagerId == userId;
            }
            else if (participantRole == Permissions.Approver)
            {
                return (f) => f.Definition.ApproverId == userId;
            }

            return (f) => false;
        }


        public static Expression<Func<Form, bool>> GetFilterExpression(long userId, GlobalAccess gAccess)
        {

            Expression<Func<Form, bool>> expr = (Form f) => GetParticipationResult(f, userId) ||
                                                            GetLocalAccessResult(f, userId) ||
                                                            GetGlobalAccessResult(f, gAccess);
            return expr;
        }
        public static bool GetFilterExpressionResults(Form f, long userId, GlobalAccess globalAccess)
        {
            bool participationResult = GetParticipationResult(f, userId);
            bool localAccessResult = GetLocalAccessResult(f, userId);
            bool globalAccessResult = GetGlobalAccessResult(f, globalAccess);

            bool result = participationResult || localAccessResult || globalAccessResult;

            return result;
        }
        public static bool GetParticipationResult(Form f, long userId)
        {
            return f.Definition.EmployeeId == userId ||
                   f.Definition.ManagerId == userId ||
                   f.Definition.ApproverId == userId;
        }
        public static bool GetLocalAccessResult(Form f, long userId)
        {
            return f.LocalAccesses.Any(la => la.UserId == userId);
        }
        public static bool GetGlobalAccessResult(Form f, GlobalAccess gAccess)
        {
            if (gAccess.DepartmentId == null)
            {
                return true;
            }
            else if (gAccess.TeamId == null)
            {
                return f.Definition.Employee.DepartmentId == gAccess.DepartmentId;
            }
            else if (gAccess.WorkprojectId == null)
            {
                return f.Definition.Employee.DepartmentId == gAccess.DepartmentId &&
                       f.Definition.Employee.TeamId == gAccess.TeamId;
            }
            else
            {
                return f.Definition.Employee.DepartmentId == gAccess.DepartmentId &&
                       f.Definition.Employee.TeamId == gAccess.TeamId &&
                       f.Definition.WorkprojectId == gAccess.WorkprojectId;
            }
        }


        public static Func<Form, bool> GetParticipationMethod(long userId)
        {
            return (f) => f.Definition.EmployeeId == userId ||
                          f.Definition.ManagerId == userId ||
                          f.Definition.ApproverId == userId;
        }
        public static Func<Form, bool> GetLocalAccessMethod(long userId)
        {
            return (f) => f.LocalAccesses.Any(la => la.UserId == userId);
        }
        public static Func<Form, bool> GetGlobalAccessMethod(GlobalAccess gAccess)
        {
            if (gAccess.DepartmentId == null)
            {
                return (f) => true;
            }
            else if (gAccess.TeamId == null)
            {
                return (f) => f.Definition.Employee.DepartmentId == gAccess.DepartmentId;
            }
            else if (gAccess.WorkprojectId == null)
            {
                return (f) => f.Definition.Employee.DepartmentId == gAccess.DepartmentId &&
                              f.Definition.Employee.TeamId == gAccess.TeamId;
            }
            else
            {
                return (f) => f.Definition.Employee.DepartmentId == gAccess.DepartmentId &&
                              f.Definition.Employee.TeamId == gAccess.TeamId &&
                              f.Definition.WorkprojectId == gAccess.WorkprojectId;
            }
        }
    }
}
