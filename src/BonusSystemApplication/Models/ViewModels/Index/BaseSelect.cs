namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class BaseSelect
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BaseSelect(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
