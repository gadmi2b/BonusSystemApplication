namespace BonusSystemApplication.Models
{
    public class Form
    {
        public long Id { get; set; }
        public bool IsObjectivesFreezed { get; set; }
        public bool IsResultsFreezed { get; set; }
        public DateTime? LastSavedDateTime { get; set; }
        public string? LastSavedBy { get; set; }

        public Definition Definition { get; set; }
        public IList<ObjectiveResult> ObjectivesResults { get; set; }
        public Conclusion Conclusion { get; set; }
        public Signatures Signatures { get; set; }
        public ICollection<LocalAccess> LocalAccesses { get; set; }
    }
}
