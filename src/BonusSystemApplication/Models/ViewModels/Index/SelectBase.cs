namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class SelectBase
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SelectBase(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
