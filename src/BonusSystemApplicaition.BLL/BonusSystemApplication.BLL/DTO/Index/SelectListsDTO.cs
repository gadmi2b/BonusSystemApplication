using BonusSystemApplication.BLL.Processes.Filtering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BonusSystemApplication.BLL.DTO.Index
{
    public class SelectListsDTO
    {
        public List<SelectListItem> EmployeeSelectListItems { get; set; }
        public List<SelectListItem> PeriodSelectListItems { get; set; }
        public List<SelectListItem> YearSelectListItems { get; set; }
        public List<SelectListItem> PermissionSelectListItems { get; set; }
        public List<SelectListItem> DepartmentSelectListItems { get; set; }
        public List<SelectListItem> TeamSelectListItems { get; set; }
        public List<SelectListItem> WorkprojectSelectListItems { get; set; }


        public SelectListsDTO(FormDataAvailable formDataAvailable,
                                   UserSelectionsDTO userSelections)
        {
            EmployeeSelectListItems = PrepareSelectListItems(formDataAvailable.AvailableEmployees,
                                                             userSelections.SelectedEmployees);
            PeriodSelectListItems = PrepareSelectListItems(formDataAvailable.AvailablePeriods,
                                                           userSelections.SelectedPeriods);
            YearSelectListItems = PrepareSelectListItems(formDataAvailable.AvailableYears,
                                                         userSelections.SelectedYears);
            PermissionSelectListItems = PrepareSelectListItems(formDataAvailable.AvailablePermissions,
                                                               userSelections.SelectedPermissions);
            DepartmentSelectListItems = PrepareSelectListItems(formDataAvailable.AvailableDepartments,
                                                               userSelections.SelectedDepartments);
            TeamSelectListItems = PrepareSelectListItems(formDataAvailable.AvailableTeams,
                                                         userSelections.SelectedTeams);
            WorkprojectSelectListItems = PrepareSelectListItems(formDataAvailable.AvailableWorkprojects,
                                                                userSelections.SelectedWorkprojects);
        }

        private List<SelectListItem> PrepareSelectListItems<T>(List<T> collectionAvailable,
                                                               List<string> selectedValues)
        {
            #region Get expression depending on collection type to cast collection item to string
            Type listType = typeof(T);
            Func<T, string> expr = (T param) => string.Empty;

            if (listType.IsEnum)
            {
                expr = (T param) => Enum.GetName(typeof(T), param);
            }
            else if (listType == typeof(int) ||
                     listType == typeof(long))
            {
                expr = (T param) => param.ToString();
            }
            else if (listType == typeof(string))
            {
                expr = (T param) => param.ToString();
            }
            else
            {
                throw new Exception($"PrepareSelectListItems error: unexpected type: {typeof(T)}." +
                                    $"An additional case to operate this type should be added");
            }
            #endregion

            #region Prepare selectListItems
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (T availableItem in collectionAvailable)
            {
                string itemName = expr.Invoke(availableItem);
                SelectListItem selectListItem = new SelectListItem
                {
                    Value = itemName,
                    Text = itemName,
                    Selected = false,
                    Disabled = false,
                };

                if (selectedValues.Contains(itemName))
                {
                    selectListItem.Selected = true;
                }

                selectListItems.Add(selectListItem);
            }
            #endregion

            return selectListItems;
        }

    }
}
