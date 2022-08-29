namespace BonusSystemApplication.Models
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
