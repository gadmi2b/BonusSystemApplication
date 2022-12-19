namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class UserSelections
    {
        public List<string> SelectedEmployees { get; set; } = new List<string>();
        public List<string> SelectedPeriods { get; set; } = new List<string>();
        public List<string> SelectedYears { get; set; } = new List<string>();
        public List<string> SelectedPermissions { get; set; } = new List<string>();
        public List<string> SelectedDepartments { get; set; } = new List<string>();
        public List<string> SelectedTeams { get; set; } = new List<string>();
        public List<string> SelectedWorkprojects { get; set; } = new List<string>();

        
        public void PrepareSelections(FormDataAvailable formDataAvailable)
        {
            RemoveDistinct();

            SetStandardOrder(SelectedEmployees);
            SetStandardOrder(SelectedPeriods);
            SetStandardOrder(SelectedYears);
            SetStandardOrder(SelectedPermissions);
            SetStandardOrder(SelectedDepartments);
            SetStandardOrder(SelectedTeams);
            SetStandardOrder(SelectedWorkprojects);

            ValidateSelections(formDataAvailable);
        }
        private void RemoveDistinct()
        {
            SelectedEmployees = SelectedEmployees.Distinct().ToList();
            SelectedPeriods = SelectedPeriods.Distinct().ToList();
            SelectedYears = SelectedYears.Distinct().ToList();
            SelectedPermissions = SelectedPermissions.Distinct().ToList();
            SelectedDepartments = SelectedDepartments.Distinct().ToList();
            SelectedTeams = SelectedTeams.Distinct().ToList();
            SelectedWorkprojects = SelectedWorkprojects.Distinct().ToList();
        }
        private void SetStandardOrder(List<string> SelectedCollection)
        {
            SelectedCollection.RemoveAll(x => x == string.Empty);
            SelectedCollection.OrderByDescending(x => x);
            SelectedCollection.Insert(0, string.Empty);
        }
        private void ValidateSelections(FormDataAvailable formDataAvailable)
        {
            List<string> itemsToRemove = new List<string>();

            //Selected Employees validation
            foreach (string item in SelectedEmployees)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formDataAvailable.AvailableEmployees.Contains(item))
                {
                    //SelectedEmployees.Remove(item);
                    itemsToRemove.Add(item);
                }
            }
            SelectedEmployees.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Periods validation
            foreach (string item in SelectedPeriods)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : (Enum.TryParse(item, out Periods result) && !formDataAvailable.AvailablePeriods.Contains(result)) ||
                      !Enum.TryParse(item, out result))
                {
                    //SelectedPeriods.Remove(item);
                    itemsToRemove.Add(item);
                }
            }
            SelectedPeriods.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Years validation
            foreach (string item in SelectedYears)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : (Int32.TryParse(item, out int result) && !formDataAvailable.AvailableYears.Contains(result)) ||
                      !Int32.TryParse(item, out result))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedYears.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Permissions validation
            foreach (string item in SelectedPermissions)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : (Enum.TryParse(item, out Permission result) && !formDataAvailable.AvailablePermissions.Contains(result)) ||
                      !Enum.TryParse(item, out result))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedPermissions.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Departments validation
            foreach (string item in SelectedDepartments)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formDataAvailable.AvailableDepartments.Contains(item))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedDepartments.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Teams validation
            foreach (string item in SelectedTeams)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formDataAvailable.AvailableTeams.Contains(item))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedTeams.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();

            //Selected Workprojects validation
            foreach (string item in SelectedWorkprojects)
            {
                if (string.IsNullOrEmpty(item)
                    ? false
                    : !formDataAvailable.AvailableWorkprojects.Contains(item))
                {
                    itemsToRemove.Add(item);
                }
            }
            SelectedWorkprojects.RemoveAll(x => itemsToRemove.Contains(x));
            itemsToRemove.Clear();
        }
    }
}
