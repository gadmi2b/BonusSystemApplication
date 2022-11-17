﻿namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class PropertyLinker : IPropertyLinker
    {
        public PropertyTypes PropertyType { get; set; }
        public Dictionary<string, string?> IdPairsIsSignedIsRejected { get; set; }

        public Dictionary<string, string?> IdPairsIsSignedSignature { get; set; }
    }
}
