namespace BonusSystemApplication.BLL.UserIdentiry
{
    /// <summary>
    /// This class should be replaced by normal user identity system
    /// </summary>
    public static class UserData
    {
        static long _userId { get; set; } = 14;
        static string _userName { get; set; } = "Nikolay Nikolayev";
        public static List<long> AvailableFormIds { get; set; } = new List<long>();

        public static long GetUserId() => _userId;
        public static string GetUserName() => _userName;
        public static string GetUserSignature() => $"{_userName} {DateTime.Now}";

    }
}
