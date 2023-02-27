using BonusSystemApplication.BLL.Processes.Signing;

namespace BonusSystemApplication.BLL.Interfaces
{
    public interface IPropertyLinker
    {
        PropertyType PropertyType { get; set; }

        // link between all IsSigned and IsRejected properties
        Dictionary<string, string?> IdPairsIsSignedIsRejected { get; set; }
        // link between all IsSigned and Signature properties
        Dictionary<string, string?> IdPairsIsSignedSignature { get; set; }
    }
}
