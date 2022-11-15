using System.Reflection;

namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class FormSaver
    {
        public static void SaveSignature(Form form, string signatureCheckboxId, bool isSignatureCheckboxChecked)
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
        }

        private static void SetFormPropertyValueByName(Form form, string propertyName, object propertyValue)
        {
            PropertyInfo propertyInfo = form.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if(propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(form, propertyValue);
            }
        }
    }
}
