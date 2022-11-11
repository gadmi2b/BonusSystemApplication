using System.Reflection;

namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class Saver
    {
        public static void SaveSignature(Form form, string signatureCheckboxId, bool isSignatureCheckboxChecked)
        {
            /*
             *   IsObjectivesSignedByEmployee: if(isSignatureCheckboxChecked)  { IsObjectivesSignedByEmployee = true and 
             *                                                                 ObjectivesEmployeeSignature = signature }
             *   IsObjectivesSignedByEmployee: if(!isSignatureCheckboxChecked) { IsObjectivesSignedByEmployee = false and 
             *                                                                 ObjectivesEmployeeSignature = string.Empty
             *                                                                 IsObjectivesRejectedByEmployee = false }
            */
        }

        private static void SetPropertyValueByName(Form form, string propertyName, object propertyValue)
        {
            PropertyInfo propertyInfo = form.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if(propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(form, propertyValue);
            }
        }
    }
}
