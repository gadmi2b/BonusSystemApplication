namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public interface IPropertyLinker
    {
        PropertyTypes PropertyType { get; set; }

        // link between all IsSigned and IsRejected properties
        Dictionary<string, string?> IsSignedIsRejectedPairs { get; set; }
        // link between all IsSigned and Signature properties
        Dictionary<string, string?> IsSignedSignaturePairs { get; set; }
    }
}
