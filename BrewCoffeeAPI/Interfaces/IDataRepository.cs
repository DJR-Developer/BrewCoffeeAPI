namespace CoffeeAPI.Interfaces
{
    /// <summary>
    /// Data Repository interface
    /// </summary>
    public interface IDataRepository
    {
        public int GetValue();
        public bool SetValue(int number);
    }
}
