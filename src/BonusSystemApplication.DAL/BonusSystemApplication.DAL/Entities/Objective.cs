namespace BonusSystemApplication.DAL.Entities
{
    public class Objective
    {
        public string? Statement { get; set; }
        public string? Description { get; set; }
        public bool IsKey { get; set; } = true;
        public bool IsMeasurable { get; set; } = true;
        public string? Unit { get; set; }
        public double? Threshold { get; set; }
        public double? Target { get; set; }
        public double? Challenge { get; set; }
        public double? WeightFactor { get; set; }
        public double? KpiUpperLimit { get; set; }
    }
}
