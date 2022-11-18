using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.Models.ViewModels.Index
{
    public static class UserData
    {
        public static long UserId { get; set; }
        static string UserName { get; set; } = "Current ApplicationUser";
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

        public static string GetUserSignature()
        {
            return $"{UserName} {DateTime.Now}";
        }

        public static string GetUserName()
        {
            return UserName;
        }
    }
}
