namespace CoffeeAPI.Classes
{
    /// <summary>
    /// Configurable settings
    /// </summary>
    public class Settings
    {
        public string PDFileName { get; set; }
        public string DateOutputFormat { get; set; }
        public int OutOfOrderDay { get; set; }
        public int OutOfOrderMonth { get; set; }
        public int MaxCounterValue { get; set; }
    }
}
