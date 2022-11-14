namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public class SignaturePropertiesAnalyser
    {
        private IPropertyLinker AffectedPropertyLinker { get; } = null;

        public SignaturePropertiesAnalyser(List<IPropertyLinker> signaturePropertyLinkers,
                                                          string signatureCheckboxId)
        {
            foreach(IPropertyLinker pLinker in signaturePropertyLinkers)
            {
                if (pLinker.IsSignedIsRejectedPairs.Keys.Contains(signatureCheckboxId) ||
                    pLinker.IsSignedIsRejectedPairs.Values.Contains(signatureCheckboxId))
                {
                    AffectedPropertyLinker = pLinker;
                    break;
                }
            }
        }

        public List<Dictionary<string, object?>> GetPropertiesValuesToSet(Form form,
                                                                          string signatureCheckboxId,
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
            if (IsSignatureImpossible(form))
            {
                return propertiesValuesToSet;
            }

            // TODO: write code here
            Dictionary<string, object?> propertyValue = new Dictionary<string, object?>();


            return propertiesValuesToSet;

        }
        private bool IsSignatureImpossible(Form form)
        {
            // signing/dropping already signed/dropped positions will not be checked

            // objectives signing is allowed only
            if (form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                if (AffectedPropertyLinker!.GetType() == typeof(ObjectivesSignaturePropertyLinker))
                    return true;
            }

            // results signing is allowed only
            if (form.IsObjectivesFreezed && form.IsResultsFreezed)
            {
                if (AffectedPropertyLinker!.GetType() == typeof(ResultsSignaturePropertyLinker))
                    return true;
            }

            return false;
        }
    }
}
