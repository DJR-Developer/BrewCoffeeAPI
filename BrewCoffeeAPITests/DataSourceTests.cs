using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using CoffeeAPI.Classes;


namespace ReadyTechAPITests
{
    public class DataSourceTests
    {
        private string filePath;
        private Mock<ILogger<DataSource>> _logger;
        private IOptions<Settings> _mockSettings;
        private DataSource _dataSource;


        [SetUp]
        public void Setup()
        {
            // Set basic settings
            var settings = new Settings
            {
                PDFileName = "testing.txt",
                OutOfOrderDay = 1,
                OutOfOrderMonth = 4,
                DateOutputFormat = "yyyy-MM-ddTHH:mm:ssK",
                MaxCounterValue = 5
            };

            _mockSettings = Options.Create(settings);
            _logger = new Mock<ILogger<DataSource>>();
            _dataSource = new DataSource(_logger.Object, _mockSettings);
            
            filePath = AppContext.BaseDirectory + "\\" + settings.PDFileName;
        }


        /// <summary>
        /// Clean up by removing test file
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            File.Delete(filePath);
        }


        #region GetValue tests
        /// <summary>
        /// Test Get Value with no file existing, should return null
        /// </summary>
        [Test]
        public void TestGetValueNoFileExists()
        {
            // Given, When
            int? result = _dataSource.GetValue();

            // Then
            Assert.That(result, Is.Null);
        }


        /// <summary>
        /// Test GetValue with a value already set, should return number set
        /// </summary>
        [Test]
        public void TestGetValueValid()
        {
            // Given
            int number = 1;

            // Need to set it so we can retrieve it
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(number);
                }
            }

            // When
            var result = _dataSource.GetValue();

            // Then
            Assert.That(result, Is.EqualTo(number));
        }
        #endregion


        #region SetValue tests
        /// <summary>
        /// Test SetValue returns success on valid usage
        /// </summary>
        [Test]
        public void TestSetValueValid()
        {
            // Given
            int number = 1;

            // When
            var result = _dataSource.SetValue(number);

            // Then
            Assert.That(result, Is.EqualTo(true));
        }


        /// <summary>
        /// Test SetValue fails if it has an issue setting the value
        /// </summary>
        [Test]
        public void TestSetValueInvalid()
        {
            // Given
            _dataSource.filePath = "<>.txt"; // Set an invalid filename so that code breaks
            int number = 1;

            // When
            var result = _dataSource.SetValue(number);

            // Then
            Assert.That(result, Is.EqualTo(false));
        }
        #endregion
    }
}