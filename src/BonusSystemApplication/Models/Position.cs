namespace BonusSystemApplication.Models
{
    public class Position
    {
        public long Id { get; set; }
        public string NameEng { get; set; }
        public string NameRus { get; set; }
        public string? Abbreviation { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
