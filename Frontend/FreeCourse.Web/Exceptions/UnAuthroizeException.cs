namespace FreeCourse.Web.Exceptions
{
    public class UnAuthroizeException : Exception
    {
        public UnAuthroizeException()
        {
        }

        public UnAuthroizeException(string? message) : base(message)
        {
        }

        public UnAuthroizeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
