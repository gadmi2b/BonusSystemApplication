﻿using BonusSystemApplication.Models;
using BonusSystemApplication.Models.Repositories;

namespace BonusSystemApplication.UserIdentiry
{
    public static class UserData
    {
        public static long UserId { get; set; }
        static string UserName { get; set; } = "Current ApplicationUser";
        public static List<long> AvailableFormIds { get; set; } = new List<long>();

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
