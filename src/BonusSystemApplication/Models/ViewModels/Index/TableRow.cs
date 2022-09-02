﻿namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class TableRow : ITableRow
    {
        public long Id { get; set; }
        public string WorkprojectName { get; set; }
        public string EmployeeFullName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public int Year { get; set; }
        public Periods Period { get; set; }
        public DateTime? LastSavedDateTime { get; set; }
        public List<AccessFilter> AccessFilters { get; set; } = new List<AccessFilter>();

        /// <summary>
        /// determines in according to which criteria this form is available for user
        /// </summary>
        /// <param name="tableRows">collection of TableRows to operate</param>
        /// <param name="formIdsWithGlobalAccess">collection of form's ids related to global access</param>
        /// <param name="formIdsWithLocalAccess">collection of form's ids related to local access</param>
        /// <param name="formIdsWithEmployeeParticipation">collection of form's ids related to participation as Employee</param>
        /// <param name="formIdsWithManagerParticipation">collection of form's ids related to participation as Manger</param>
        /// <param name="formIdsWithApproverParticipation">collection of form's ids related to participation as Approver</param>
        public static void IdentifyAccessFilters(List<TableRow> tableRows,
                                                  List<long> formIdsWithGlobalAccess,
                                                  List<long> formIdsWithLocalAccess,
                                                  List<long> formIdsWithEmployeeParticipation,
                                                  List<long> formIdsWithManagerParticipation,
                                                  List<long> formIdsWithApproverParticipation)
        {
            foreach (TableRow vm in tableRows)
            {
                if (formIdsWithGlobalAccess.Contains(vm.Id))
                {
                    vm.AccessFilters.Add(AccessFilter.GlobalAccess);
                }
                if (formIdsWithLocalAccess.Contains(vm.Id))
                {
                    vm.AccessFilters.Add(AccessFilter.LocalAccess);
                }
                if (formIdsWithEmployeeParticipation.Contains(vm.Id))
                {
                    vm.AccessFilters.Add(AccessFilter.Employee);
                }
                if (formIdsWithManagerParticipation.Contains(vm.Id))
                {
                    vm.AccessFilters.Add(AccessFilter.Manager);
                }
                if (formIdsWithApproverParticipation.Contains(vm.Id))
                {
                    vm.AccessFilters.Add(AccessFilter.Approver);
                }
            }
        }
    }
}