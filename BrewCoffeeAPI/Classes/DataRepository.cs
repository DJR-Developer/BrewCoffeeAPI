using CoffeeAPI.Interfaces;

namespace CoffeeAPI.Classes
{
    /// <summary>
    /// Data Repository class
    /// </summary>
    public class DataRepository : IDataRepository
    {
        public readonly IDataSource _dataSource;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Custom app settings</param>
        public DataRepository(IDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }


        /// <summary>
        /// Retrieves the current counter value
        /// </summary>
        /// <returns>The retrieved value or zero on error</returns>
        public int GetValue()
        {
            int? number = _dataSource.GetValue();
            return number == null ? 0 : Convert.ToInt32(number);
        }


        /// <summary>
        /// Updates and sets specified value in the data source
        /// </summary>
        /// <param name="number">Value to set</param>
        /// <returns>True on success, false on failure</returns>
        public bool SetValue(int number)
        {
            return _dataSource.SetValue(number);
        }
    }
}
