﻿namespace BonusSystemApplication.Models.BusinessLogic.SignatureProcess
{
    public class PropertyLinker : IPropertyLinker
    {
        public PropertyType PropertyType { get; set; }
        public string PropertyTypeName { get; set; }
        public Dictionary<string, string?> IdPairsIsSignedIsRejected { get; set; }

        public Dictionary<string, string?> IdPairsIsSignedSignature { get; set; }
    }
}