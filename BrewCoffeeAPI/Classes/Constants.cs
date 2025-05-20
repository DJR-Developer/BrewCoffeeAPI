namespace CoffeeAPI.Classes
{
    /// <summary>
    /// Error messages and strings used by the API. 
    /// Moving strings to a single class can make localisation easier.
    /// </summary>
    public class Constants
    {
        public static readonly string SuccessMessage = "Success";
        public static readonly string ErrorMessage = "Error";
        public static readonly string ResponseMessage = "Your piping hot coffee is ready";
        public static readonly string UnavailableMessage = "Service Unavailable";
        public static readonly string OutOfOrderMessage = "I'm a teapot";
        public static readonly string InternalServerErrorMessage = "Internal Server Error";
        public static readonly string CounterGetErrorMessage = "Unable to retrieve Counter value";
        public static readonly string CounterSetErrorMessage = "Unable to set Counter value";
    }
}