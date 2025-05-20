using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CoffeeAPI.Classes;
using CoffeeAPI.Interfaces;
using System.Text.Json;

namespace CoffeeAPI.Controllers
{
    /// <summary>
    /// Controller for Brew Coffee API calls
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BrewCoffeeController : ControllerBase
    {
        private readonly ILogger<BrewCoffeeController> _logger;
        private readonly IDataRepository _dataRepository;
        private readonly Settings _settings;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="dataRepository">Data Repositorys</param>
        /// <param name="settings">Custom AppSettings</param>
        public BrewCoffeeController(ILogger<BrewCoffeeController> logger, IDataRepository dataRepository, IOptions<Settings> settings)
        {
            _logger = logger?? throw new ArgumentNullException(nameof(logger));
            _dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }


        /// <summary>
        /// Get API call
        /// </summary>
        /// <returns>HTTP Status code and relevant message</returns>
        [HttpGet("/brew-coffee", Name = "brew-coffee")]
        public ObjectResult Get()
        {
            DateTime today = DateTime.Now;
            int counter = _dataRepository.GetValue();

            // Data Repository or Data Source not working, return internal error
            if (counter == 0)
            {
                return StatusCode(500, Constants.InternalServerErrorMessage);
            }

            // Check if machine is out of order
            if (today.Day == _settings.OutOfOrderDay && today.Month == _settings.OutOfOrderMonth)
            {
                _logger.LogError($"{Constants.ErrorMessage}: {Constants.OutOfOrderMessage}");
                return StatusCode(418, string.Empty);
            }
            // Service Unavailable every X calls (default 5 but configurable)
            else if (counter == _settings.MaxCounterValue)
            {
                UpdateCounter(counter);

                _logger.LogError($"{Constants.ErrorMessage}: {Constants.UnavailableMessage}");
                return StatusCode(503, string.Empty);
            }

            // Send success response
            BrewCoffeeResponseItem bcResponseItem = new()
            {
                Message = Constants.ResponseMessage,
                Prepared = DateTime.Now.ToString(_settings.DateOutputFormat, System.Globalization.CultureInfo.InvariantCulture)
            };

            UpdateCounter(counter);
            
            _logger.LogInformation($"{Constants.SuccessMessage}: {JsonSerializer.Serialize(bcResponseItem)}");
            return StatusCode(200, bcResponseItem);
        }


        /// <summary>
        /// Increments Counter value between 1 and MaxCounterValue
        /// </summary>
        /// <param name="counter">Counter value</param>
        internal void UpdateCounter(int counter)
        {
            counter++;

            if (counter > _settings.MaxCounterValue) counter = 1;

            if (!_dataRepository.SetValue(counter))
            {
                _logger.LogError($"{Constants.ErrorMessage}: {Constants.CounterGetErrorMessage}");
            }
        }
    }
}