namespace BonusSystemApplication.BLL.Infrastructure
{
    /// <summary>
    /// The exception that is thrown on BLL layer when Presentation layer
    /// sends data which is not in line with Business rules
    /// </summary>
    public class ValidationException : Exception
    {
        public string Property { get; protected set; }

        public ValidationException() { }
        public ValidationException(string message, string property) : base(message)
        {
            Property = property;
        }
        public ValidationException(string message, string property, Exception innerException) : base(message, innerException)
        {
            Property = property;
        }
    }
}
