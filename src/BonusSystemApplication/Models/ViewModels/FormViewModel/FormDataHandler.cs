using BonusSystemApplication.Models.ViewModels.Index;
using System.Reflection;

namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class FormDataHandler
    {
        public static bool IsObjectivesSignaturePossible(Form form)
        {
            if(form.IsObjectivesFreezed && !form.IsResultsFreezed)
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
    
        public static void PrepareSignatureData(ref Dictionary<string, object> propertiesValues)
        {
            if(propertiesValues == null)
            {
                throw new ArgumentNullException(nameof(propertiesValues));
            }

            // LOGIC: find the first bool and and get its value inside of all values
            //        there are could be max two bools with same values (isSignedId and isRejectedId)
            //        and one string value (signatureId)
            bool isSigned = false;
            foreach(var value in propertiesValues.Values)
            {
                if(value != null &&
                   value.GetType() == typeof(bool))
                {
                    isSigned = (bool)value;
                    break;
                }
            }

            string userSignature = string.Empty;
            if(isSigned)
            {
                userSignature = UserData.GetUserSignature();
            }

            // LOGIC: find the first string value inside all values
            //        and assign a User signature to it
            foreach (string key in propertiesValues.Keys)
            {
                if(propertiesValues.TryGetValue(key, out var value))
                {
                    if (value.GetType() == typeof(string))
                    {
                        propertiesValues[key] = userSignature;
                        break;
                    }
                }
            }
        }
    
        public static void UpdateSignatureFormData(Form form, Dictionary<string, object> propertiesValues)
        {
            foreach(string property in propertiesValues.Keys)
            {
                var value = propertiesValues[property];
                PropertyInfo propertyInfo = form.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(form, value);
                }
            }
        }
        public static void UpdateLastSavedFormData(Form form)
        {
            form.LastSavedBy = UserData.GetUserName();
            form.LastSavedDateTime = DateTime.Now;
        }
    }
}