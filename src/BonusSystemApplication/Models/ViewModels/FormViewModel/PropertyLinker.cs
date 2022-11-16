namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class PropertyLinker : IPropertyLinker
    {
        public PropertyTypes PropertyType { get; set; }
        public Dictionary<string, string?> IsSignedIsRejectedPairs { get; set; }

        public Dictionary<string, string?> IsSignedSignaturePairs { get; set; }
    }
}
