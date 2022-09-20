namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class Objective
    {
        public long Id { get; set; } = 0;
        public int Row { get; set; } = 0;
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
