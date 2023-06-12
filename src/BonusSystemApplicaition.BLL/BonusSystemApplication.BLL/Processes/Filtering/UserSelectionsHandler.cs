using BonusSystemApplication.BLL.DTO.Index;
using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.Processes.Filtering
{
  public class UserSelectionsHandler
  {
        FormDataAvailable _formDataAvailable { get; set; }
        public UserSelectionsHandler(FormDataAvailable formDataAvailable)
        {
            _formDataAvailable = formDataAvailable;
        }

        public void PrepareSelections(UserSelectionsDTO userSelections)
        {
            userSelections.SelectedEmployees = RemoveDuplicates(userSelections.SelectedEmployees);
            userSelections.SelectedPeriods = RemoveDuplicates(userSelections.SelectedPeriods);
            userSelections.SelectedYears = RemoveDuplicates(userSelections.SelectedYears);
            userSelections.SelectedPermissions = RemoveDuplicates(userSelections.SelectedPermissions);
            userSelections.SelectedDepartments = RemoveDuplicates(userSelections.SelectedDepartments);
            userSelections.SelectedTeams = RemoveDuplicates(userSelections.SelectedTeams);
            userSelections.SelectedWorkprojects = RemoveDuplicates(userSelections.SelectedWorkprojects);

            SetStandardOrder(userSelections.SelectedEmployees);
            SetStandardOrder(userSelections.SelectedPeriods);
            SetStandardOrder(userSelections.SelectedYears);
            SetStandardOrder(userSelections.SelectedPermissions);
            SetStandardOrder(userSelections.SelectedDepartments);
            SetStandardOrder(userSelections.SelectedTeams);
            SetStandardOrder(userSelections.SelectedWorkprojects);

            ValidateEmployees(userSelections.SelectedEmployees);
            ValidatePeriods(userSelections.SelectedPeriods);
            ValidateYears(userSelections.SelectedYears);
            ValidatePermissions(userSelections.SelectedPermissions);
            ValidateDepartments(userSelections.SelectedDepartments);
            ValidateTeams(userSelections.SelectedTeams);
            ValidateWorkprojects(userSelections.SelectedWorkprojects);
        }

        private List<string> RemoveDuplicates(List<string> selectedCollection)
        {
            return selectedCollection.Distinct().ToList();
        }
        private void SetStandardOrder(List<string> selectedCollection)
        {
            selectedCollection.RemoveAll(x => x == string.Empty);
            selectedCollection.OrderByDescending(x => x);
            selectedCollection.Insert(0, string.Empty);
        }
        private void ValidateEmployees(List<string> selectedCollection)
        {
            List<string> itemsToRemove = new List<string>();
            foreach (string item in selectedCollection)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                if (!_formDataAvailable.AvailableEmployees.Contains(item))
                    itemsToRemove.Add(item);
            }

            selectedCollection.RemoveAll(x => itemsToRemove.Contains(x));
        }
        private void ValidatePeriods(List<string> selectedCollection)
        {
            List<string> itemsToRemove = new List<string>();
            foreach (string item in selectedCollection)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                Periods result;
                if (!Enum.TryParse(item, out result))
                {
                    itemsToRemove.Add(item);
                    continue;
                }

                if (!_formDataAvailable.AvailablePeriods.Contains(result))
                    itemsToRemove.Add(item);
            }

            selectedCollection.RemoveAll(x => itemsToRemove.Contains(x));
        }
        private void ValidateYears(List<string> selectedCollection)
        {
            List<string> itemsToRemove = new List<string>();
            foreach (string item in selectedCollection)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                int result;
                if (!int.TryParse(item, out result))
                {
                    itemsToRemove.Add(item);
                    continue;
                }

                if (!_formDataAvailable.AvailableYears.Contains(result))
                    itemsToRemove.Add(item);
            }

            selectedCollection.RemoveAll(x => itemsToRemove.Contains(x));
        }
        private void ValidatePermissions(List<string> selectedCollection)
        {
            List<string> itemsToRemove = new List<string>();
            foreach (string item in selectedCollection)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                Permission result;
                if (!Enum.TryParse(item, out result))
                {
                    itemsToRemove.Add(item);
                    continue;
                }

                if (!_formDataAvailable.AvailablePermissions.Contains(result))
                    itemsToRemove.Add(item);
            }

            selectedCollection.RemoveAll(x => itemsToRemove.Contains(x));
        }
        private void ValidateDepartments(List<string> selectedCollection)
        {
            List<string> itemsToRemove = new List<string>();
            foreach (string item in selectedCollection)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                if (!_formDataAvailable.AvailableDepartments.Contains(item))
                    itemsToRemove.Add(item);
            }

            selectedCollection.RemoveAll(x => itemsToRemove.Contains(x));
        }
        private void ValidateTeams(List<string> selectedCollection)
        {
            List<string> itemsToRemove = new List<string>();
            foreach (string item in selectedCollection)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                if (!_formDataAvailable.AvailableTeams.Contains(item))
                    itemsToRemove.Add(item);
            }

            selectedCollection.RemoveAll(x => itemsToRemove.Contains(x));
        }
        private void ValidateWorkprojects(List<string> selectedCollection)
        {
            List<string> itemsToRemove = new List<string>();
            foreach (string item in selectedCollection)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                if (!_formDataAvailable.AvailableWorkprojects.Contains(item))
                    itemsToRemove.Add(item);
            }

            selectedCollection.RemoveAll(x => itemsToRemove.Contains(x));
        }
    }
}
