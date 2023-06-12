namespace BonusSystemApplication.DAL.Entities
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<User>? Users { get; set; }
    }
}
