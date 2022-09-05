namespace BonusSystemApplication.Models.ViewModels.Index
{
    public class FormDataSingleton
    {
        private static bool isCreated = false;
        List<Form> AvailableForms { get; }

        public FormDataSingleton(List<Form> availableForms)
        {
            if (isCreated)
            {
                throw new Exception("Only one instance of object is allowed to be created");
            }

            AvailableForms = availableForms;

            isCreated = true;
        }
    }
}
