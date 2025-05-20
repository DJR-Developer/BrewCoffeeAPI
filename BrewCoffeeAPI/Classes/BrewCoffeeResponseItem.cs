using System.Text.Json.Serialization;

namespace CoffeeAPI.Classes
{
    /// <summary>
    /// Response class for JSON outputted by Brew Coffee API
    /// </summary>
    public class BrewCoffeeResponseItem
    {
        /// <summary>
        /// Response message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }


        /// <summary>
        /// Date request made
        /// </summary>
        [JsonPropertyName("prepared")] 
        public string Prepared { get; set; }
    }
}
