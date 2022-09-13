using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public static class UserData
    {
        public static long UserId { get; set; }
        public static List<long> availableFormIds { get; private set; } = new List<long>();

        public static void SetAvailableFormIds(List<Form> availableForms)
        {
            if(availableFormIds.Count > 0)
            {
                availableFormIds.Clear();
            }

            foreach(Form form in availableForms)
            {
                availableFormIds.Add(form.Id);
            }
        }
    }
}
