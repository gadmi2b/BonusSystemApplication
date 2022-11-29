namespace BonusSystemApplication.Models
{
    public class Objective
    {
        public string Statement { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsKey { get; set; } = true;
        public bool IsMeasurable { get; set; } = true;
        public string Unit { get; set; } = string.Empty;
        public string Threshold { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;
        public string WeightFactor { get; set; } = string.Empty;
        public string KpiUpperLimit { get; set; } = string.Empty;
    }
}
