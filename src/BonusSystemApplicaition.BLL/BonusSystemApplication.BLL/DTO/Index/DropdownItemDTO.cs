namespace BonusSystemApplication.BLL.DTO.Index
{
    public class DropdownItemDTO
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; } = false;
        public bool Disabled { get; set; } = false;
    }
}
