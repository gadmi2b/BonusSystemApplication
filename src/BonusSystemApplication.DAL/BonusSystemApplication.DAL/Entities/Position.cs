namespace BonusSystemApplication.DAL.Entities
{
    public class Position
    {
        public long Id { get; set; }
        public string NameEng { get; set; }
        public string NameRus { get; set; }
        public string? Abbreviation { get; set; }

        public List<User>? Users { get; set; }
    }
}
