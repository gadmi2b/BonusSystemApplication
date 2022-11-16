namespace BonusSystemApplication.Models.ViewModels.FormViewModel
{
    public static class FormChecker
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
    }
}
