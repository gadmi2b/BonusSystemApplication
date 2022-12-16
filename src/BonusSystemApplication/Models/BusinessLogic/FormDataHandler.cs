using BonusSystemApplication.UserIdentiry;
using Microsoft.Data.SqlClient.Server;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;

namespace BonusSystemApplication.Models.BusinessLogic
{
    public static class FormDataHandler
    {
        public static bool IsObjectivesSignaturePossible(Form form)
        {
            if (form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }
        public static bool IsResultsSignaturePossible(Form form)
        {
            if (form.IsObjectivesFreezed && form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }

        public static void PutUserSignature(ref Dictionary<string, object> propertiesValues)
        {
            if (propertiesValues == null)
            {
                throw new ArgumentNullException(nameof(propertiesValues));
            }

            #region Description of logic
            /*
             * LOGIC: find the first bool and and get its value inside of all values
             *        there are could be max two bools with same values (isSignedId and isRejectedId)
             *        and one string value (signatureId)
             */
            #endregion

            bool isSigned = false;
            foreach (var value in propertiesValues.Values)
            {
                if (value != null &&
                   value.GetType() == typeof(bool))
                {
                    isSigned = (bool)value;
                    break;
                }
            }

            string userSignature = string.Empty;
            if (isSigned)
            {
                userSignature = UserData.GetUserSignature();
            }

            #region Description of logic
            /*
             * LOGIC: find the first string value inside all values
             *        and assign a User signature to it
             */
            #endregion

            foreach (string key in propertiesValues.Keys)
            {
                if (propertiesValues.TryGetValue(key, out var value))
                {
                    if (value.GetType() == typeof(string))
                    {
                        propertiesValues[key] = userSignature;
                        break;
                    }
                }
            }
        }

        public static void UpdateLastSavedFormData(Form form)
        {
            form.LastSavedBy = UserData.GetUserName();
            form.LastSavedDateTime = DateTime.Now;
        }

        public static void UpdateSignatures(Form form, Dictionary<string, object> propertiesValues)
        {
            Signatures signatures = form.Signatures;
            foreach (string propertyPath in propertiesValues.Keys)
            {
                var value = propertiesValues[propertyPath];
                SetNestedProperty(propertyPath, signatures, value);
            }

            form.Signatures = signatures;
        }

        /// <summary>
        /// Sets property of nested type of target object by propertyPath
        /// </summary>
        /// <param name="propertyPath"></param> like ChildObject.Property
        /// <param name="target"></param> Parent object
        /// <param name="value"></param> new value of property
        private static void SetNestedProperty(string propertyPath, object target, object value)
        {
            string[] levels = propertyPath.Split('.');
            for (int lvl = 0; lvl < levels.Length - 1; lvl++)
            {
                PropertyInfo propertyToGet = target.GetType().GetProperty(levels[lvl]);
                target = propertyToGet.GetValue(target, null);
            }
            PropertyInfo propertyToSet = target.GetType().GetProperty(levels.Last());
            propertyToSet.SetValue(target, value, null);
        }

 
        private static bool IsObjectivesResultsSavePossible(Form form)
        {
            if(!form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }
        private static bool IsResultsSavePossible(Form form)
        {
            if (form.IsObjectivesFreezed && !form.IsResultsFreezed)
            {
                return true;
            }
            return false;
        }
    }
}