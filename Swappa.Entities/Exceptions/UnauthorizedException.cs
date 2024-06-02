namespace Swappa.Entities.Exceptions
{
    public class UnauthorizedException : Exception
    {
        protected UnauthorizedException(string message) : base(message) { }
    }
}
