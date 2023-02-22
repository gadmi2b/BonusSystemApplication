namespace BonusSystemApplication.DAL.Entities
{
    public class Objective
    {
        public string? Statement { get; set; }
        public string? Description { get; set; }
        public bool IsKey { get; set; } = true;
        public bool IsMeasurable { get; set; } = true;
        public string? Unit { get; set; }
        public string? Threshold { get; set; }
        public string? Target { get; set; }
        public string? Challenge { get; set; }
        public string? WeightFactor { get; set; }
        public string? KpiUpperLimit { get; set; }
    }
}
