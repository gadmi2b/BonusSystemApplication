namespace BonusSystemApplication.DAL.Entities
{
    public class ForResults
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
            return $"{nameof(ForResults)}.{nameof(EmployeeSignature)}";
        }
        public static string ToStringIsSignedByEmployee()
        {
            return $"{nameof(ForResults)}.{nameof(IsSignedByEmployee)}";
        }
        public static string ToStringIsRejectedByEmployee()
        {
            return $"{nameof(ForResults)}.{nameof(IsRejectedByEmployee)}";
        }
        public static string ToStringManagerSignature()
        {
            return $"{nameof(ForResults)}.{nameof(ManagerSignature)}";
        }
        public static string ToStringIsSignedByManager()
        {
            return $"{nameof(ForResults)}.{nameof(IsSignedByManager)}";
        }
        public static string ToStringApproverSignature()
        {
            return $"{nameof(ForResults)}.{nameof(ApproverSignature)}";
        }
        public static string ToStringIsSignedByApprover()
        {
            return $"{nameof(ForResults)}.{nameof(IsSignedByApprover)}";
        }
    }
}
