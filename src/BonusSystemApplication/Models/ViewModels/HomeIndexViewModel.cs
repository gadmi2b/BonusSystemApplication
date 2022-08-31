using BonusSystemApplication.Models.ViewModels.Index;

namespace BonusSystemApplication.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public TableFilters TableFilters { get; set; }
        public List<TableRow> TableRows { get; set; }
    }
}
