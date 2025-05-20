namespace CoffeeAPI.Interfaces
{
    /// <summary>
    /// Data Source interface
    /// </summary>
    public interface IDataSource
    {
        public int? GetValue();
        public bool SetValue(int x);
    }
}
