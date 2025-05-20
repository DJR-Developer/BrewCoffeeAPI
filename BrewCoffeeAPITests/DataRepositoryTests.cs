using Moq;
using CoffeeAPI.Classes;
using CoffeeAPI.Interfaces;


namespace ReadyTechAPITests
{
    public class DataRepositoryTests
    {
        private Mock<IDataSource> _mockDataSource;
        private DataRepository _repository;


        [SetUp]
        public void Setup()
        {
            _mockDataSource = new Mock<IDataSource>();
            _repository = new DataRepository(_mockDataSource.Object) ?? throw new ArgumentNullException(nameof(_repository));
        }


        /// <summary>
        /// Test Get Value returns integer on success
        /// </summary>
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void TestGetValueValid(int value)
        {
            // Given
            _mockDataSource.Setup(ds => ds.GetValue()).Returns(value);

            // When
            var result = _repository.GetValue();

            // Then
            Assert.That(result, Is.EqualTo(value));
        }


        /// <summary>
        /// Test GetValue returns 0 on error
        /// </summary>
        [Test]
        public void TestGetValueInvalid()
        {
            // Given
            int value = 0;
            _mockDataSource.Setup(ds => ds.GetValue()).Returns((int?) null);

            // When
            var result = _repository.GetValue();

            // Then
            Assert.That(result, Is.EqualTo(value));
        }


        /// <summary>
        /// Ensure SetValue returns True on success
        /// </summary>
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void TestSetValueValid(int value)
        {
            // Given
            _mockDataSource.Setup(ds => ds.SetValue(value)).Returns(true);

            // When
            var result = _repository.SetValue(value);

            // Then
            Assert.True(result);
        }


        /// <summary>
        /// Ensure SetValue return False on failure
        /// </summary>
        [Test]
        public void TestSetValueInvalid()
        {
            // Given
            int value = 1;
            _mockDataSource.Setup(ds => ds.SetValue(value)).Returns(false);

            // When
            var result = _repository.SetValue(value);

            // Then
            Assert.False(result);
        }
    }
}