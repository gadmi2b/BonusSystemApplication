namespace BonusSystemApplication.DAL.Entities
{
    public class Department
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
