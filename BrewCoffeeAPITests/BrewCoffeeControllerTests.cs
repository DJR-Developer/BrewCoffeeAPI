using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using CoffeeAPI.Classes;
using CoffeeAPI.Controllers;
using CoffeeAPI.Interfaces;
using System.Globalization;


namespace ReadyTechAPITests
{
    public class BrewCoffeeControllerTests
    {
        private Mock<ILogger<BrewCoffeeController>> _mockBCLogger;
        private Mock<IDataRepository> _mockDataRepository;
        private IOptions<Settings> _mockSettings;
        private BrewCoffeeController _brewCoffeeController;

        [SetUp]
        public void Setup()
        {
            var settings = new Settings
            {
                PDFileName = "testing.txt",
                OutOfOrderDay = 1,
                OutOfOrderMonth = 4,
                DateOutputFormat = "yyyy-MM-ddTHH:mm:ssK",
                MaxCounterValue = 5
            };

            _mockSettings = Options.Create(settings);
            _mockBCLogger = new Mock<ILogger<BrewCoffeeController>>();
            _mockDataRepository = new Mock<IDataRepository>();

            _brewCoffeeController = new BrewCoffeeController(_mockBCLogger.Object, _mockDataRepository.Object, _mockSettings) ?? throw new ArgumentNullException(nameof(_brewCoffeeController));
        }


        /// <summary>
        /// Test Internal Server error happens when expected
        /// </summary>
        [Test]
        public void TestGetValueInternalError()
        {
            // Given
            _mockDataRepository.Setup(dr => dr.GetValue()).Returns(0);

            // When
            var result = _brewCoffeeController.Get();

            // Then
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }


        /// <summary>
        /// Test the Out of Service date
        /// </summary>
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestOutOfService(int value)
        {
            // Given
            _mockSettings.Value.OutOfOrderMonth = DateTime.Now.Month;
            _mockSettings.Value.OutOfOrderDay = DateTime.Now.Day;
            _mockDataRepository.Setup(dr => dr.GetValue()).Returns(value);
            _mockDataRepository.Setup(dr => dr.SetValue(value)).Returns(true);

            // When
            var result = _brewCoffeeController.Get();                   
            
            // Then
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status418ImATeapot));
        }


        /// <summary>
        /// Test for a Service Unavailable that happens every Max (5th by default) turn
        /// </summary>
        [Test]
        public void TestServiceUnavailable()
        {
            // Given
            int number = _mockSettings.Value.MaxCounterValue;
            _mockDataRepository.Setup(dr => dr.GetValue()).Returns(number);

            // When
            var result = _brewCoffeeController.Get();

            // Then
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status503ServiceUnavailable));
        }

        
        /// <summary>
        /// Test for a valid 200 OK result
        /// </summary>
        /// <param name="number">Number to test</param>
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestOKResult(int number)
        {
            // Given
            _mockDataRepository.Setup(dr => dr.GetValue()).Returns(number);
            _mockDataRepository.Setup(dr => dr.SetValue(number)).Returns(true);

            // When
            var result = _brewCoffeeController.Get();

            // Then
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }


        /// <summary>
        /// Test response Message output
        /// </summary>
        /// <param name="number">Number</param>
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestValidMessageOutput(int number)
        {
            // Given
            _mockDataRepository.Setup(dr => dr.GetValue()).Returns(number);
            _mockDataRepository.Setup(dr => dr.SetValue(number)).Returns(true);

            // When
            var result = _brewCoffeeController.Get();
                   
            // Then
            Assert.That((result.Value as BrewCoffeeResponseItem).Message, Is.EqualTo(Constants.ResponseMessage));
        }


        /// <summary>
        /// Test response Prepared output
        /// </summary>
        /// <param name="number"></param>
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestValidPreparedOutput(int number)
        {
            // Given
            _mockDataRepository.Setup(dr => dr.GetValue()).Returns(number);
            _mockDataRepository.Setup(dr => dr.SetValue(number)).Returns(true);

            // When
            var output = _brewCoffeeController.Get();

            bool result = DateTime.TryParseExact((output.Value as BrewCoffeeResponseItem).Prepared, _mockSettings.Value.DateOutputFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

            // Then
            Assert.That(result, Is.True);
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestCounterUpdateVerify(int number)
        {
            // Given
            int expectedResult = number + 1;
            expectedResult = expectedResult == 6 ? expectedResult = 1 : expectedResult;

            _mockDataRepository.Setup(dr => dr.SetValue(number)).Returns(true);

            // When
            _brewCoffeeController.UpdateCounter(number);

            // Then (
            _mockDataRepository.Verify(dr => dr.SetValue(expectedResult), Times.Once);
        }
    }
}