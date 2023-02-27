using BonusSystemApplication.Models.ViewModels.IndexViewModel;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public TableSelectListsVM TableSelectLists { get; set; }
        public List<TableRowVM> TableRows { get; set; }
    }
}
