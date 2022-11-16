namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class PropertyLinkerHandler
    {
        public static IPropertyLinker AffectedPropertyLinker { get; set; } = null;

        public static bool IsPropertyLinkerAffected(IPropertyLinker propertyLinker,
                                                             string signatureCheckboxId)
        {
            if(propertyLinker == null || string.IsNullOrEmpty(signatureCheckboxId))
            {
                return false;
            }

            if (propertyLinker.IsSignedIsRejectedPairs.Keys.Contains(signatureCheckboxId) ||
                propertyLinker.IsSignedIsRejectedPairs.Values.Contains(signatureCheckboxId))
            {
                AffectedPropertyLinker = propertyLinker;
                return true;
            }
            return false;
        }

        public static List<Dictionary<string, object?>> GetPropertiesValues(string signatureCheckboxId,
                                                                            bool isSignatureCheckboxChecked)
        {
            /*   Signed / Rejected pairs:
             *   IsObjectivesSignedByEmployee: if(!isSignatureCheckboxChecked) { IsObjectivesSignedByEmployee = false and 
             *                                                                 ObjectivesEmployeeSignature = string.Empty
             *                                                                 IsObjectivesRejectedByEmployee = false }
             *   IsObjectivesRejectedByEmployee: if(!isSignatureCheckboxChecked) { IsObjectivesSignedByEmployee = false and 
             *                                                                 ObjectivesEmployeeSignature = string.Empty
             *                                                                 IsObjectivesRejectedByEmployee = false }                                                              
             *   Other variants:
             *   IsObjectivesSignedByXXX: if(isSignatureCheckboxChecked)  { IsObjectivesSignedByXXX = true and 
             *                                                              ObjectivesXXXSignature = signature }
            */

            // if signatureCheckboxId in IsSignedIsRejectedPairs in Keys:
            // => logic applied for all SignedBy properties: sign/drop with signature
            // if for this Key exists Value and
            // if(!isSignatureCheckboxChecked) => we have to drop IsRejected property also

            // if signatureCheckboxId in IsSignedIsRejectedPairs in Values:
            // => logic applied for all SignedBy properties: sign/drop, but without signature

            //if (ObjectivesIsSignedIsRejectedPropNamesPairs.Keys.Contains(signatureCheckboxId) ||
            //    ResultsIsSignedIsRejectedPropNamesPairs.Keys.Contains(signatureCheckboxId))
            //{
            //    formPropNameValue.Add(signatureCheckboxId, isSignatureCheckboxChecked);
            //}

            List<Dictionary<string, object?>> propertiesValuesToSet = new List<Dictionary<string, object?>>();
            if(AffectedPropertyLinker == null || string.IsNullOrEmpty(signatureCheckboxId))
            {
                return propertiesValuesToSet;
            }

            // logic:
            // if checkbox for signing was affected then we have to add key-value pair to List
            // in this case if this checkbox has a reject pair then
            // if signature was dropped => we have to drop reject checkbox also => add key-value pair to List


            if (AffectedPropertyLinker.IsSignedIsRejectedPairs.ContainsKey(signatureCheckboxId))
            {
                propertiesValuesToSet.Add(new Dictionary<string, object?> { { signatureCheckboxId, isSignatureCheckboxChecked } });

                if(AffectedPropertyLinker.IsSignedIsRejectedPairs.TryGetValue(signatureCheckboxId, out string rejectedById))
                {
                    if (!isSignatureCheckboxChecked)
                    {
                        propertiesValuesToSet.Add(new Dictionary<string, object?> { { rejectedById, isSignatureCheckboxChecked } });
                    }
                }
            }

            // TODO: logic:
            // if checkbox for reject was affected then add key-value pair
            // in this case if(isSignatureCheckboxChecked) and
            // there is no signature then we have to put it also => add key-value pair to List

            if (AffectedPropertyLinker.IsSignedIsRejectedPairs.ContainsValue(signatureCheckboxId))
            {
                propertiesValuesToSet.Add(new Dictionary<string, object?> { { signatureCheckboxId, isSignatureCheckboxChecked } });
            }

            return propertiesValuesToSet;

        }
    }
}
