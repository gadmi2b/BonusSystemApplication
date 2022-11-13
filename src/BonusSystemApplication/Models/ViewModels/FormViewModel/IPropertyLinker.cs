namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public interface IPropertyLinker
    {
        // link between all IsSigned and IsRejected properties
        Dictionary<string, string?> IsSignedIsRejectedPairs { get; }
        // link between all IsSigned and Signature properties
        Dictionary<string, string?> IsSignedSignaturePairs { get; }
    }
}
