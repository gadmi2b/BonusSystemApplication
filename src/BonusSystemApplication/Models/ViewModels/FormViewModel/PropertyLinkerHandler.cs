namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class PropertyLinkerHandler
    {
        public static IPropertyLinker AffectedPropertyLinker { get; set; } = null;

        public static bool IsPropertyLinkerAffected(IPropertyLinker propertyLinker,
                                                             string checkboxId)
        {
            if (propertyLinker == null || string.IsNullOrEmpty(checkboxId))
            {
                return false;
            }

            if (propertyLinker.IdPairsIsSignedIsRejected.ContainsKey(checkboxId) ||
                propertyLinker.IdPairsIsSignedIsRejected.ContainsValue(checkboxId))
            {
                AffectedPropertyLinker = propertyLinker;
                return true;
            }
            return false;
        }

        public static Dictionary<string, object> GetPropertiesValues(string checkboxId,
                                                                     bool isCheckboxChecked)
        {
            Dictionary<string, object> propertiesValues = new Dictionary<string, object>();
            if (AffectedPropertyLinker == null || string.IsNullOrEmpty(checkboxId))
            {
                return propertiesValues;
            }

            // logic:
            // if checkbox for signing was clicked then we have to add:
            // - isSigned  key-value pair to dictionary (id and value)
            // - signature key-value pair to dictionary (id and just empty.string)
            // in this case:
            //      if this checkbox has a pair with rejectId then
            //      if signature was dropped => we have to drop reject checkbox also => add:
            //      - IsRejected key-value pair to dictionary (id and value)


            if (AffectedPropertyLinker.IdPairsIsSignedIsRejected.ContainsKey(checkboxId))
            {
                propertiesValues.Add(checkboxId, isCheckboxChecked);
                if (AffectedPropertyLinker.IdPairsIsSignedSignature
                    .TryGetValue(checkboxId, out string signatureId)){
                    propertiesValues.Add(signatureId, string.Empty);
                }

                if (!isCheckboxChecked &&
                    AffectedPropertyLinker.IdPairsIsSignedIsRejected
                    .TryGetValue(checkboxId, out string isRejectedId)) // return null to string and true in TryGetValue - to check
                {
                    propertiesValues.Add(isRejectedId, isCheckboxChecked);
                }
            }

            // logic:
            // if checkbox for rejecting was clicked then we have to add:
            // - isRejected key-value pair to dictionary (id and value)
            // in this case:
            //      if reject was initiated (true) then we have also change IsSigned => add
            //      - IsSigned key-value pair to dictionary (id and value)
            //      - signature key-value pair to dictionary (id and just empty.string) 

            if (AffectedPropertyLinker.IdPairsIsSignedIsRejected.ContainsValue(checkboxId))
            {
                propertiesValues.Add(checkboxId, isCheckboxChecked);
                if (isCheckboxChecked)
                {
                    string isSignedId = AffectedPropertyLinker.IdPairsIsSignedIsRejected
                                            .First(e => e.Value == checkboxId).Key;

                    if (!string.IsNullOrEmpty(isSignedId))
                    {
                        propertiesValues.Add(isSignedId, isCheckboxChecked);
                        if (AffectedPropertyLinker.IdPairsIsSignedSignature
                            .TryGetValue(isSignedId, out string signatureId))
                        {
                            propertiesValues.Add(signatureId, string.Empty);
                        }
                    }
                }
            }

            return propertiesValues;
        }
    }
}
