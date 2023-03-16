using BonusSystemApplication.BLL.DTO.Edit;

namespace BonusSystemApplication.Models.Forms.Edit
{
    public class ObjectiveVM
    {
        public string Statement { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsKey { get; set; } = true;
        public bool IsMeasurable { get; set; } = true;
        public string Unit { get; set; } = string.Empty;
        public string? Threshold { get; set; } = null;
        public string? Target { get; set; } = null;
        public string? Challenge { get; set; } = null;
        public string? WeightFactor { get; set; } = null;
        public string? KpiUpperLimit { get; set; } = null;

        public ObjectiveVM() { }
    }
}
