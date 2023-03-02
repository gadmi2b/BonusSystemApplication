using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class ObjectiveDTO
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

        //public ObjectiveDTO(Objective source)
        //{
        //    Statement = source.Statement == null ? string.Empty : source.Statement;
        //    Description = source.Description == null ? string.Empty : source.Description;
        //    IsKey = source.IsKey;
        //    IsMeasurable = source.IsMeasurable;
        //    Unit = source.Unit == null ? string.Empty : source.Unit;
        //    Threshold = source.Threshold == null ? string.Empty : source.Threshold;
        //    Target = source.Target == null ? string.Empty : source.Target;
        //    Challenge = source.Challenge == null ? string.Empty : source.Challenge;
        //    WeightFactor = source.WeightFactor == null ? string.Empty : source.WeightFactor;
        //    KpiUpperLimit = source.KpiUpperLimit == null ? string.Empty : source.KpiUpperLimit;
        //}
    }
}
