namespace BonusSystemApplication.Models
{
    public class Workproject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Form>? Forms { get; set; }
    }
}
