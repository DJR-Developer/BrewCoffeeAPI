using Microsoft.Extensions.Options;
using CoffeeAPI.Interfaces;

namespace CoffeeAPI.Classes
{
    /// <summary>
    /// Data Source class - can be replaced by any kind of data source without
    /// needing to modify the rest of the application
    /// </summary>
    public class DataSource : IDataSource
    {
        public string filePath;
        public readonly ILogger<IDataSource> _logger;
        public readonly Settings _settings;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings">Custom app settings</param>
        public DataSource(ILogger<IDataSource> logger, IOptions<Settings> settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));

            filePath = AppContext.BaseDirectory + "\\" + _settings.PDFileName;
        }


        /// <summary>
        /// Retrieves the current counter value
        /// </summary>
        /// <returns>Value on success, null on a failure</returns>
        public string? Read()
        {
            try
            {
                // Open file and read value
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        return sr.ReadLine();
                    }
                }
            }
            // If any errors occur, log and return null
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, Constants.ErrorMessage + ": " + Constants.CounterGetErrorMessage + "\n" + e.Message);
                return null;
            }

        }


        /// <summary>
        /// Updates and sets specified value in the file
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <returns>True on success, false on failure</returns>
        public bool Write(string value)
        {
            try
            {
                // Open file and write the new value
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(value);
                    }

                    return true;
                }
            }
            // On any error log and return false
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, Constants.ErrorMessage + ": " + Constants.CounterSetErrorMessage + "\n" + e.Message);
                return false;
            }
        }


        /// <summary>
        /// Retrieves the current counter value
        /// </summary>
        /// <returns>Value on success, null on a failure</returns>
        public int? GetValue()
        {
            string? s = Read();

            if (s == null) return null;

            return Convert.ToInt32(s);
        }


        /// <summary>
        /// Updates and sets specified value in the file
        /// </summary>
        /// <param name="value">Value to set</param>
        /// <returns>True on success, false on failure</returns>
        public bool SetValue(int value)
        {
            return Write(value.ToString());
        }
    }
}