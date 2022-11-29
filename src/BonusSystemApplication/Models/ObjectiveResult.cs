namespace BonusSystemApplication.Models
{
    public class ObjectiveResult
    {
        public long Id { get; set; }
        public int Row { get; set; }
        public Objective Objective { get; set; }
        public Result Result { get; set; }

        public long FormId { get; set; }
        public Form Form { get; set; }
    }
}
