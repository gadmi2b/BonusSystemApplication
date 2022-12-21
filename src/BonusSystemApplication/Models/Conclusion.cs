using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BonusSystemApplication.Models
{
    public class Conclusion
    {
        public long Id { get; set; }
        public string? OverallKpi { get; set; }
        public bool IsProposalForBonusPayment { get; set; }
        public string? ManagerComment { get; set; }
        public string? EmployeeComment { get; set; }
        public string? OtherComment { get; set; }

        [BindNever]
        public Form Form { get; set; }
    }
}