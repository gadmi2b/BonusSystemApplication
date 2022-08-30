using BonusSystemApplication.Models.ViewModels;
using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<Form, bool>> GetExpressionForGlobalAccess(FormGlobalAccess formGlobalAccess)
        {
            Expression<Func<Form, bool>> expr = (Form f) => false;

            if (formGlobalAccess.DepartmentId == null)
            {
                expr = (Form f) => true;
            }
            else if (formGlobalAccess.TeamId == null)
            {
                expr = (Form f) => f.Employee.DepartmentId == formGlobalAccess.DepartmentId;
            }
            else if (formGlobalAccess.WorkprojectId == null)
            {
                expr = (Form f) => f.Employee.DepartmentId == formGlobalAccess.DepartmentId &&
                                   f.Employee.TeamId == formGlobalAccess.TeamId;
            }
            else
            {
                expr = (Form f) => f.Employee.DepartmentId == formGlobalAccess.DepartmentId &&
                                   f.Employee.TeamId == formGlobalAccess.TeamId &&
                                   f.WorkprojectId == formGlobalAccess.WorkprojectId;
            }

            return expr;
        }
        public static Expression<Func<Form, bool>> GetExpressionForLocalAccess(long userId)
        {
            Expression<Func<Form, bool>> expr = (Form f) => f.FormLocalAccess.Any(fla => fla.UserId == userId);
            return expr;
        }

        // TODO: integrate GetMethodForParticipation into GetExpressionForParticipation
        public static Expression<Func<Form, bool>> GetExpressionForParticipation(long userId)
        {
            Expression<Func<Form, bool>> expr = (Form f) => f.EmployeeId == userId || f.ManagerId == userId || f.ApproverId == userId;
            return expr;
        }

        public static Func<Form, bool> GetMethodForParticipation(long userId, AccessFilter participantRole)
        {
            if(participantRole == AccessFilter.Employee)
            {
                return (f) => f.EmployeeId == userId;
            }
            else if (participantRole == AccessFilter.Manager)
            {
                return (f) => f.ManagerId == userId;
            }
            else if (participantRole == AccessFilter.Approver)
            {
                return (f) => f.ApproverId == userId;
            }

            return (f) => false;
        }
    }
}
