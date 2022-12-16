﻿using BonusSystemApplication.Models.ViewModels.Index;
using System.Linq.Expressions;

namespace BonusSystemApplication.Models.Repositories
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<Form, bool>> GetExpressionForGlobalAccess(GlobalAccess globalAccess)
        {
            Expression<Func<Form, bool>> expr = (Form f) => false;

            if (globalAccess.DepartmentId == null)
            {
                expr = (Form f) => true;
            }
            else if (globalAccess.TeamId == null)
            {
                expr = (Form f) => f.Definition.Employee.DepartmentId == globalAccess.DepartmentId;
            }
            else if (globalAccess.WorkprojectId == null)
            {
                expr = (Form f) => f.Definition.Employee.DepartmentId == globalAccess.DepartmentId &&
                                   f.Definition.Employee.TeamId == globalAccess.TeamId;
            }
            else
            {
                expr = (Form f) => f.Definition.Employee.DepartmentId == globalAccess.DepartmentId &&
                                   f.Definition.Employee.TeamId == globalAccess.TeamId &&
                                   f.Definition.WorkprojectId == globalAccess.WorkprojectId;
            }

            return expr;
        }
        public static Expression<Func<Form, bool>> GetExpressionForLocalAccess(long userId)
        {
            Expression<Func<Form, bool>> expr = (Form f) => f.LocalAccesses.Any(la => la.UserId == userId);
            return expr;
        }

        // TODO: integrate GetMethodForParticipation into GetExpressionForParticipation
        public static Expression<Func<Form, bool>> GetExpressionForParticipation(long userId)
        {
            Expression<Func<Form, bool>> expr = (Form f) => f.Definition.EmployeeId == userId ||
                                                            f.Definition.ManagerId == userId ||
                                                            f.Definition.ApproverId == userId;
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
    }
}
