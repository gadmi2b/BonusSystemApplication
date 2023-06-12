using BonusSystemApplication.BLL.Interfaces;

namespace BonusSystemApplication.BLL.Processes.Signing
{
    public class PropertyLinkerHandler
    {
        public IPropertyLinker? AffectedPropertyLinker { get; set; } = null;

        public bool IsPropertyLinkerAffected(IPropertyLinker propertyLinker,
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
        public Dictionary<string, object> GetPropertiesValues(string checkboxId,
                                                                       bool isCheckboxChecked)
        {
            Dictionary<string, object> propertiesValues = new Dictionary<string, object>();
            if (AffectedPropertyLinker == null || string.IsNullOrEmpty(checkboxId))
            {
                return propertiesValues;
            }

            #region Description of logic
            /*
             * LOGIC: if signature was dropped => reject must be dropped also
             * if checkbox for signing was clicked then we have to add:
             * - isSigned  key-value pair to dictionary (id and value)
             * - signature key-value pair to dictionary (id and just empty.string)
             * in this case:
             *     if this checkbox has a pair with rejectId then
             *     if signature was dropped => we have to drop reject checkbox also => add:
             *     - IsRejected key-value pair to dictionary (id and value)
             */
            #endregion

            if (AffectedPropertyLinker.IdPairsIsSignedIsRejected.ContainsKey(checkboxId))
            {
                propertiesValues.Add(checkboxId, isCheckboxChecked);

                if (IsExistGetMember(AffectedPropertyLinker.IdPairsIsSignedSignature,
                                    checkboxId, out string signatureId))
                {
                    propertiesValues.Add(signatureId, string.Empty);
                }

                if (!isCheckboxChecked &&
                    IsExistGetMember(AffectedPropertyLinker.IdPairsIsSignedIsRejected,
                                    checkboxId, out string isRejectedId))
                {
                    propertiesValues.Add(isRejectedId, isCheckboxChecked);
                }
            }

            #region Description of logic
            /*
             * LOGIC: if rejected => must be signed
             * if checkbox for rejecting was clicked then we have to add:
             * - isRejected key-value pair to dictionary (id and value)
             * in this case:
             *      if reject was initiated (true) then we have change IsSigned also => add
             *      - IsSigned key-value pair to dictionary (id and value)
             *      - signature key-value pair to dictionary (id and just empty.string) 
             */
            #endregion

            if (AffectedPropertyLinker.IdPairsIsSignedIsRejected.ContainsValue(checkboxId))
            {
                propertiesValues.Add(checkboxId, isCheckboxChecked);

                if (isCheckboxChecked &&
                    IsExistGetMember(AffectedPropertyLinker.IdPairsIsSignedIsRejected,
                                        checkboxId, out string isSignedId))
                {
                    propertiesValues.Add(isSignedId, isCheckboxChecked);

                    if (IsExistGetMember(AffectedPropertyLinker.IdPairsIsSignedSignature,
                                        isSignedId, out string signatureId))
                    {
                        propertiesValues.Add(signatureId, string.Empty);
                    }
                }
            }

            return propertiesValues;
        }


        private bool IsExistGetMember(Dictionary<string, string?> dict,
                                             string keyOrValue, out string member)
        {
            // check existing value member by key
            if (dict.TryGetValue(keyOrValue, out member))
            {
                if (string.IsNullOrEmpty(member)) { return false; }
            }
            // check existing key member by value
            else
            {
                member = dict.First(e => e.Value == keyOrValue).Key;
                if (string.IsNullOrEmpty(member)) { return false; }
            }

            if (member == null) { return false; }
            else { return true; }
        }
    }
}
