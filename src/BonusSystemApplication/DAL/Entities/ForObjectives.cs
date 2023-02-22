namespace BonusSystemApplication.DAL.Entities
{
    public class ForObjectives
    {
        public string? EmployeeSignature { get; set; }
        public bool IsSignedByEmployee { get; set; }
        public bool IsRejectedByEmployee { get; set; }
        public string? ManagerSignature { get; set; }
        public bool IsSignedByManager { get; set; }
        public string? ApproverSignature { get; set; }
        public bool IsSignedByApprover { get; set; }

        public static string ToStringEmployeeSignature()
        {
            return $"{nameof(ForObjectives)}.{nameof(EmployeeSignature)}";
        }
        public static string ToStringIsSignedByEmployee()
        {
            return $"{nameof(ForObjectives)}.{nameof(IsSignedByEmployee)}";
        }
        public static string ToStringIsRejectedByEmployee()
        {
            return $"{nameof(ForObjectives)}.{nameof(IsRejectedByEmployee)}";
        }
        public static string ToStringManagerSignature()
        {
            return $"{nameof(ForObjectives)}.{nameof(ManagerSignature)}";
        }
        public static string ToStringIsSignedByManager()
        {
            return $"{nameof(ForObjectives)}.{nameof(IsSignedByManager)}";
        }
        public static string ToStringApproverSignature()
        {
            return $"{nameof(ForObjectives)}.{nameof(ApproverSignature)}";
        }
        public static string ToStringIsSignedByApprover()
        {
            return $"{nameof(ForObjectives)}.{nameof(IsSignedByApprover)}";
        }
    }
}