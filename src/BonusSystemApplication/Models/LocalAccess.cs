namespace BonusSystemApplication.Models
{
    public class LocalAccess
    {
        public long FormId { get; set; }
        public Form Form { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
