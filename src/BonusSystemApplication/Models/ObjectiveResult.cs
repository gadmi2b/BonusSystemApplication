namespace BonusSystemApplication.Models
{
    public class ObjectiveResult
    {
        public long Id { get; set; }
        public int Row { get; set; }
        public string? Statement { get; set; }
        public string? Description { get; set; }
        public bool IsKey { get; set; }
        public bool IsMeasurable { get; set; }
        public string? Unit { get; set; }
        public string? Threshold { get; set; }
        public string? Target { get; set; }
        public string? Challenge { get; set; }
        public string? WeightFactor { get; set; }
        public string? KpiUpperLimit { get; set; }
        public string? KeyCheck { get; set; }
        public string? Achieved { get; set; }
        public string? Kpi { get; set; }

        public long FormId { get; set; }
        public Form Form { get; set; }
    }
}
