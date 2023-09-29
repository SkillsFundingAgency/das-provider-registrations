using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Services;
using System;
using System.Net.Http;

namespace SFA.DAS.ProviderRegistrations.UnitTests.Services
{
    public class TrainingProviderApiClientFactoryTest
    {
        private const string OuterApiBaseAddress = "http://outer-api";
        private Mock<HttpMessageHandler> _mockHttpsMessageHandler = null!;
        private Fixture _fixture = null!;
        private TrainingProviderApiClientFactory _sut = null!;
        private TrainingProviderApiClientConfiguration _settings = null!;
        private Mock<IConfiguration> _configuration = null!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mockHttpsMessageHandler = new Mock<HttpMessageHandler>();
            _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "EnvironmentName")]).Returns("LOCAL");
            _settings = _fixture
                .Build<TrainingProviderApiClientConfiguration>()
                .With(x => x.ApiBaseUrl, OuterApiBaseAddress)
                .With(x => x.IdentifierUri, "")
                .Create();

            _sut = new TrainingProviderApiClientFactory(new HttpClient(_mockHttpsMessageHandler.Object)
            {
                BaseAddress = new Uri(OuterApiBaseAddress),
            }, _settings, _configuration.Object);
        }

        [Test]
        public void When_CreateHttpClient_Then_HttpClient_IsReturned()
        {
            // Act
            var actual = _sut.CreateHttpClient();

            // Assert
            actual.Should().NotBeNull();
            actual.DefaultRequestHeaders.Should().NotBeNull();
            actual.DefaultRequestHeaders.Should().HaveCountGreaterOrEqualTo(1);
        }
    }
}
