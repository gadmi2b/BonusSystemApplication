namespace BonusSystemApplication.Models.Forms.Index
{
    public class FormIndexViewModel
    {
        public DropdownListsVM DropdownLists { get; set; } = new DropdownListsVM();
        public List<TableRowVM> TableRows { get; set; } = new List<TableRowVM>();
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
