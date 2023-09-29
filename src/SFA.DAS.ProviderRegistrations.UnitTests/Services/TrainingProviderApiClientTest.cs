using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Models;
using SFA.DAS.ProviderRegistrations.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Services
{
    public class TrainingProviderApiClientTest
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private Mock<HttpMessageHandler> _mockHttpsMessageHandler = null!;
        private Fixture _fixture = null!;
        private TrainingProviderApiClient _sut = null!;
        private TrainingProviderApiClientConfiguration _settings = null!;
        private Mock<ILogger<TrainingProviderApiClient>> _logger = null!;
        private ITrainingProviderApiClientFactory _factory = null!;
        private Mock<IConfiguration> _configuration = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttpsMessageHandler = new Mock<HttpMessageHandler>();
            _logger = new Mock<ILogger<TrainingProviderApiClient>>();
            _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "EnvironmentName")]).Returns("LOCAL");
            _settings = _fixture
                .Build<TrainingProviderApiClientConfiguration>()
                .With(x => x.ApiBaseUrl, OuterApiBaseAddress)
                .With(x => x.IdentifierUri, "")
                .Create();
        }

        [Test]
        public async Task When_ProviderDetails_Found_Then_Data_FromOuterApiIsReturned()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            var expected = _fixture.Create<GetProviderSummaryResult>();

            _mockHttpsMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expected)),
                    RequestMessage = new HttpRequestMessage()
                });
            _factory = new TrainingProviderApiClientFactory(new HttpClient(_mockHttpsMessageHandler.Object)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            }, _settings, _configuration.Object);
            _sut = new TrainingProviderApiClient(_factory, _settings, _logger.Object);

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task When_ProviderDetails_NotFound_Then_Data_FromOuterApiIsNotFound()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();
            _mockHttpsMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(""),
                    RequestMessage = new HttpRequestMessage()
                });
            _factory = new TrainingProviderApiClientFactory(new HttpClient(_mockHttpsMessageHandler.Object)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            }, _settings, _configuration.Object);
            _sut = new TrainingProviderApiClient(_factory, _settings, _logger.Object);

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeNull();
        }

        [Test]
        public async Task When_ProviderDetails_InternalServerError_Then_Data_FromOuterApiIsNotFound()
        {
            // Arrange
            var ukprn = _fixture.Create<long>();

            _mockHttpsMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(""),
                    RequestMessage = new HttpRequestMessage()
                });
            _factory = new TrainingProviderApiClientFactory(new HttpClient(_mockHttpsMessageHandler.Object)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            }, _settings, _configuration.Object);
            _sut = new TrainingProviderApiClient(_factory, _settings, _logger.Object);

            // Act
            var actual = await _sut.GetProviderDetails(ukprn);

            // Assert
            actual.Should().BeNull();
        }
    }
}
