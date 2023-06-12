using BonusSystemApplication.BLL.DTO.Index;

namespace BonusSystemApplication.BLL.Processes.Filtering
{
    public class DropdownListsCreator
    {
        FormDataAvailable _formDataAvailable { get; set; }
        UserSelectionsDTO _userSelections { get; set; }

        public DropdownListsCreator(FormDataAvailable formDataAvailable,
                                    UserSelectionsDTO userSelections)
        {
            _formDataAvailable = formDataAvailable;
            _userSelections = userSelections;
        }

        public List<DropdownItemDTO> CreateEmployeeDropdownLists()
        {
            return PrepareDropdownList(_formDataAvailable.AvailableEmployees,
                                       _userSelections.SelectedEmployees);
        }
        public List<DropdownItemDTO> CreatePeriodDropdownLists()
        {
            return PrepareDropdownList(_formDataAvailable.AvailablePeriods,
                                       _userSelections.SelectedPeriods);
        }
        public List<DropdownItemDTO> CreateYearDropdownLists()
        {
            return PrepareDropdownList(_formDataAvailable.AvailableYears,
                                       _userSelections.SelectedYears);
        }
        public List<DropdownItemDTO> CreatePermissionDropdownLists()
        {
            return PrepareDropdownList(_formDataAvailable.AvailablePermissions,
                                       _userSelections.SelectedPermissions);
        }
        public List<DropdownItemDTO> CreateDepartmentDropdownLists()
        {
            return PrepareDropdownList(_formDataAvailable.AvailableDepartments,
                                       _userSelections.SelectedDepartments);
        }
        public List<DropdownItemDTO> CreateTeamDropdownLists()
        {
            return PrepareDropdownList(_formDataAvailable.AvailableTeams,
                                       _userSelections.SelectedTeams);
        }
        public List<DropdownItemDTO> CreateWorkprojectDropdownLists()
        {
            return PrepareDropdownList(_formDataAvailable.AvailableWorkprojects,
                                       _userSelections.SelectedWorkprojects);
        }


        private List<DropdownItemDTO> PrepareDropdownList<T>(List<T> collectionAvailable,
                                                             List<string> selectedValues)
        {
            List<DropdownItemDTO> dropdownListDTOs = new List<DropdownItemDTO>();

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
                return dropdownListDTOs;
            }
            #endregion

            #region Prepare dropdownListDTOs
            foreach (T availableItem in collectionAvailable)
            {
                string itemName = expr.Invoke(availableItem);
                DropdownItemDTO selectListItem = new DropdownItemDTO
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

                dropdownListDTOs.Add(selectListItem);
            }
            #endregion

            return dropdownListDTOs;
        }
    }
}
