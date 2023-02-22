namespace BonusSystemApplication.DAL.Entities
{
    public class Conclusion
    {
        public long Id { get; set; }
        public string? OverallKpi { get; set; }
        public bool IsProposalForBonusPayment { get; set; }
        public string? ManagerComment { get; set; }
        public string? EmployeeComment { get; set; }
        public string? OtherComment { get; set; }

        public long FormId { get; set; }
        public Form Form { get; set; }
    }
}