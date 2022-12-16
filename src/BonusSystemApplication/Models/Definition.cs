﻿namespace BonusSystemApplication.Models
{
    public class Definition
    {
        public long Id { get; set; }
        public Periods Period { get; set; }
        public int Year { get; set; } = System.DateTime.Now.Year;
        public bool IsWpmHox { get; set; }
        public long EmployeeId { get; set; }
        public User Employee { get; set; }
        public long? ManagerId { get; set; }
        public User? Manager { get; set; }
        public long? ApproverId { get; set; }
        public User? Approver { get; set; }
        public long? WorkprojectId { get; set; }
        public Workproject? Workproject { get; set; }

        public Form Form { get; set; }

        //readonly values
        //public string TeamName { get; set; } = string.Empty;
        //public string PositionName { get; set; } = string.Empty;
        //public string Pid { get; set; } = string.Empty;
        //public string WorkprojectDescription { get; set; } = string.Empty;
    }
}
