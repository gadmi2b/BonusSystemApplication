namespace BonusSystemApplication.Models.BusinessLogic.SignatureProcess
{
    public interface IPropertyLinker
    {
        PropertyType PropertyType { get; set; }
        string PropertyTypeName { get; set; }

        // link between all IsSigned and IsRejected properties
        Dictionary<string, string?> IdPairsIsSignedIsRejected { get; set; }
        // link between all IsSigned and Signature properties
        Dictionary<string, string?> IdPairsIsSignedSignature { get; set; }
    }
}
