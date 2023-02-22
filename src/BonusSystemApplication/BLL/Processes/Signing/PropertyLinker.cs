using BonusSystemApplication.BLL.Interfaces;

namespace BonusSystemApplication.BLL.Processes.Signing
{
    public class PropertyLinker : IPropertyLinker
    {
        public PropertyType PropertyType { get; set; }
        public Dictionary<string, string?> IdPairsIsSignedIsRejected { get; set; }

        public Dictionary<string, string?> IdPairsIsSignedSignature { get; set; }
    }
}
