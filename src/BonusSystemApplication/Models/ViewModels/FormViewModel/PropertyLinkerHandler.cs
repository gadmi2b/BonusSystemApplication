namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class PropertyLinkerHandler
    {
        public static IPropertyLinker AffectedPropertyLinker { get; set; } = null;

        public static bool IsPropertyLinkerAffected(IPropertyLinker propertyLinker,
                                                             string signatureCheckboxId)
        {
            if (propertyLinker == null || string.IsNullOrEmpty(signatureCheckboxId))
            {
                return false;
            }

            if (propertyLinker.IdPairsIsSignedIsRejected.ContainsKey(signatureCheckboxId) ||
                propertyLinker.IdPairsIsSignedIsRejected.ContainsValue(signatureCheckboxId))
            {
                AffectedPropertyLinker = propertyLinker;
                return true;
            }
            return false;
        }

        public static Dictionary<string, object?> GetPropertiesValues(string signatureCheckboxId,
                                                                      bool isSignatureCheckboxChecked)
        {
            Dictionary<string, object?> propertiesValuesToSet = new Dictionary<string, object?>();
            if (AffectedPropertyLinker == null || string.IsNullOrEmpty(signatureCheckboxId))
            {
                return propertiesValuesToSet;
            }

            // logic:
            // if checkbox for signing was clicked then we have to add:
            // - isSigned  key-value pair to dictionary (id and value)
            // - signature key-value pair to dictionary (id and just empty.string)
            // in this case:
            //      if this checkbox has a pair with rejectId then
            //      if signature was dropped => we have to drop reject checkbox also => add:
            //      - IsRejected key-value pair to dictionary (id and value)


            if (AffectedPropertyLinker.IdPairsIsSignedIsRejected.ContainsKey(signatureCheckboxId))
            {
                propertiesValuesToSet.Add(signatureCheckboxId, isSignatureCheckboxChecked);
                if (AffectedPropertyLinker.IdPairsIsSignedSignature
                    .TryGetValue(signatureCheckboxId, out string signatureId)){
                    propertiesValuesToSet.Add(signatureId, string.Empty);
                }

                if (!isSignatureCheckboxChecked &&
                    AffectedPropertyLinker.IdPairsIsSignedIsRejected
                    .TryGetValue(signatureCheckboxId, out string isRejectedId))
                {
                    propertiesValuesToSet.Add(isRejectedId, isSignatureCheckboxChecked);
                }
            }

            // logic:
            // if checkbox for rejecting was clicked then we have to add:
            // - isRejected key-value pair to dictionary (id and value)
            // in this case:
            //      if reject was initiated (true) then we have also change IsSigned => add
            //      - IsSigned key-value pair to dictionary (id and value)
            //      - signature key-value pair to dictionary (id and just empty.string) 

            if (AffectedPropertyLinker.IdPairsIsSignedIsRejected.ContainsValue(signatureCheckboxId))
            {
                propertiesValuesToSet.Add(signatureCheckboxId, isSignatureCheckboxChecked);
                if (isSignatureCheckboxChecked)
                {
                    string isSignedId = AffectedPropertyLinker.IdPairsIsSignedIsRejected
                                            .First(e => e.Value == signatureCheckboxId).Key;

                    if (!string.IsNullOrEmpty(isSignedId))
                    {
                        propertiesValuesToSet.Add(isSignedId, isSignatureCheckboxChecked);
                        if (AffectedPropertyLinker.IdPairsIsSignedSignature
                            .TryGetValue(isSignedId, out string signatureId))
                        {
                            propertiesValuesToSet.Add(signatureId, string.Empty);
                        }
                    }
                }
            }

            return propertiesValuesToSet;
        }
    }
}
