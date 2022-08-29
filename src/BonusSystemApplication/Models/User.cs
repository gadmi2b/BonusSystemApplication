﻿namespace BonusSystemApplication.Models
{
    public class User
    {
        public long Id { get; set; }
        public string LastNameEng { get; set; }
        public string FirstNameEng { get; set; }
        public string? LastNameRus { get; set; }
        public string? FirstNameRus { get; set; }
        public string? MiddleNameRus { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Pid { get; set; }

        public long? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public long? TeamId { get; set; }
        public Team? Team { get; set; }
        public long? PositionId { get; set; }
        public Position? Position { get; set; }

        public ICollection<FormLocalAccess>? FormLocalAccess { get; set; }
        public ICollection<Form>? EmployeeForms { get; set; }
        public ICollection<Form>? ManagerForms { get; set; }
        public ICollection<Form>? ApproverForms { get; set; }
    }
}
