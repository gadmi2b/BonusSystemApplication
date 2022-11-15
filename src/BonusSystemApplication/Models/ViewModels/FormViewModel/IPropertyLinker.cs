namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public enum PropertyType
    {
        Objectives = 1,
        Results = 2,
    }

    public interface IPropertyLinker
    {
        PropertyType PropertyType { get; set; }
        // link between all IsSigned and IsRejected properties
        Dictionary<string, string?> IsSignedIsRejectedPairs { get; set; }
        // link between all IsSigned and Signature properties
        Dictionary<string, string?> IsSignedSignaturePairs { get; set; }
    }
}
