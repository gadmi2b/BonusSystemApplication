namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public long Id { get; set; }
        public string WorkprojectName { get; set; }
        public string EmployeeFullName { get; set; }
        public int Year { get; set; }
        public Periods Period { get; set; }
        public DateTime LastModified { get; set; }
        public List<AccessFilter> AccessFilters { get; set; } = new List<AccessFilter>();
    }
}
