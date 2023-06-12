namespace BonusSystemApplication.DAL.Entities
{
    public class Form
    {
        public long Id { get; set; }
        public bool AreObjectivesFrozen { get; set; }
        public bool AreResultsFrozen { get; set; }
        public DateTime? LastSavedAt { get; set; }
        public string? LastSavedBy { get; set; }

        public Definition Definition { get; set; }
        public List<ObjectiveResult> ObjectivesResults { get; set; }
        public Conclusion Conclusion { get; set; }
        public Signatures Signatures { get; set; }
        public List<LocalAccess> LocalAccesses { get; set; } = new List<LocalAccess>();
    }
}
