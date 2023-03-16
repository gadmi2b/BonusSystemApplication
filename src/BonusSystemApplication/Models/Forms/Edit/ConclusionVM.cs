using BonusSystemApplication.BLL.DTO.Edit;

namespace BonusSystemApplication.Models.Forms.Edit
{
    public class ConclusionVM
    {
        public long Id { get; set; }
        public double OverallKpi { get; set; }
        public bool IsProposalForBonusPayment { get; set; }
        public string ManagerComment { get; set; } = string.Empty;
        public string EmployeeComment { get; set; } = string.Empty;
        public string OtherComment { get; set; } = string.Empty;

        public ConclusionVM() { }
    }
}
