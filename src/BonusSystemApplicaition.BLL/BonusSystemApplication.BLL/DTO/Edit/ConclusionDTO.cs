using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class ConclusionDTO
    {
        public long Id { get; set; }
        public double OverallKpi { get; set; }
        public bool IsProposalForBonusPayment { get; set; }
        public string ManagerComment { get; set; } = string.Empty;
        public string EmployeeComment { get; set; } = string.Empty;
        public string OtherComment { get; set; } = string.Empty;

        public ConclusionDTO(Conclusion source)
        {
            Id = source.Id;

            OverallKpi = source.OverallKpi == null ? default : (double)source.OverallKpi;
            IsProposalForBonusPayment = source.IsProposalForBonusPayment;
            ManagerComment = source.ManagerComment == null ? string.Empty : source.ManagerComment;
            EmployeeComment = source.EmployeeComment == null ? string.Empty : source.EmployeeComment;
            OtherComment = source.OtherComment == null ? string.Empty : source.OtherComment;
        }
    }
}
