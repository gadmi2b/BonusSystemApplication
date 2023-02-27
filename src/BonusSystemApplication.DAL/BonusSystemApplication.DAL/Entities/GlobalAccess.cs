namespace BonusSystemApplication.DAL.Entities
{
    public class GlobalAccess
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public long? TeamId { get; set; }
        public Team? Team { get; set; }
        public long? WorkprojectId { get; set; }
        public Workproject? Workproject { get; set; }
    }
}
