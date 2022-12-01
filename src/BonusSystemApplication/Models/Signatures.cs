namespace BonusSystemApplication.Models
{
    public class Signatures
    {
        public long Id { get; set; }
        public ForObjectives ForObjectives { get; set; }
        public ForResults ForResults { get; set; }
        public long FormId { get; set; }
        public Form Form { get; set; }
    }
}
