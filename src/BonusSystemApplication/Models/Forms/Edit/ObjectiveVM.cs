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
        public string Threshold { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Challenge { get; set; } = string.Empty;
        public string WeightFactor { get; set; } = string.Empty;
        public string KpiUpperLimit { get; set; } = string.Empty;

        public ObjectiveVM() { }
        //public ObjectiveVM(ObjectiveDTO source)
        //{
        //    Statement = source.Statement;
        //    Description = source.Description;
        //    IsKey = source.IsKey;
        //    IsMeasurable = source.IsMeasurable;
        //    Unit = source.Unit;
        //    Threshold = source.Threshold;
        //    Target = source.Target;
        //    Challenge = source.Challenge;
        //    WeightFactor = source.WeightFactor;
        //    KpiUpperLimit = source.KpiUpperLimit;
        //}
    }
}
