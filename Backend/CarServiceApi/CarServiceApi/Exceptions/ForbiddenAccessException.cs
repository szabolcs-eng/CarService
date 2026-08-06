namespace CarServiceApi.Exceptions
{
    /// <summary>
    /// Thrown when an authenticated user tries to read or modify a resource
    /// (a vehicle, fuel log, or service log) that belongs to a different user.
    /// Maps to HTTP 403 in the controllers.
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException(string message) : base(message) { }
    }
}
