using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Moq;
using UrlShortener.ApplicationCore.Interfaces.Commons;
using UrlShortener.ApplicationCore.Services;
using UrlShortener.ApplicationCore.Utilities;
using UrlShorterner.API.Core.Tests.InMemory;
using UrlShortener.ApplicationCore.DTOs.Request;

namespace UrlShorterner.API.Core.Tests.Services
{
    public class UrlServiceTest
    {
        private readonly UrlService _urlService;
        private readonly Mock<IUserUrlsReader> _userUrlsReaderMock;
        private readonly InMemoryUrlDataStore _urlDataStore = [];
        private readonly FakeTimeProvider _timeProvider;
        public UrlServiceTest()
        {
            var tokenProvider = new TokenProvider();
            tokenProvider.AssignRange(1, 5);

            var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
            _timeProvider = new FakeTimeProvider();
            _userUrlsReaderMock = new Mock<IUserUrlsReader>();

            _urlService = new UrlService(shortUrlGenerator,
                    _urlDataStore,
                    _timeProvider,
                    new RedirectLinkBuilder(new Uri("https://tests/")),
                    _userUrlsReaderMock.Object);
        }

        [Fact]
        public async Task ShouldReturnShortenedUrl()
        {
            var request = CreateAddUrlRequest();

            var response = await _urlService.CreateShortenUrlAsync(request, default);

            response.Succeeded.Should().BeTrue();
            response.Value!.Id.Should().NotBeEmpty();
            response.Value!.Id.Should().Be("1");
        }

        [Fact]
        public async Task ShouldSaveShortUrl()
        {
            var request = CreateAddUrlRequest();

            var response = await _urlService.CreateShortenUrlAsync(request, default);

            response.Succeeded.Should().BeTrue();
            _urlDataStore.Should().ContainKey(response.Value!.Id);
        }

        [Fact]
        public async Task ShouldSaveShortUrlWithCreatedByAndCreatedOn()
        {
            var request = CreateAddUrlRequest();

            var response = await _urlService.CreateShortenUrlAsync(request, default);

            response.Succeeded.Should().BeTrue();
            _urlDataStore.Should().ContainKey(response.Value!.Id);
            _urlDataStore[response.Value!.Id].CreatedBy.Should().Be(request.CreatedBy);
            _urlDataStore[response.Value!.Id].CreatedOn.Should()
                .Be(_timeProvider.GetUtcNow());
        }

        [Fact]
        public async Task ShouldReturnErrorIfCreatedByIsEmpty()
        {
            var request = CreateAddUrlRequest(createdBy: string.Empty);

            var response = await _urlService.CreateShortenUrlAsync(request, default);

            response.Succeeded.Should().BeFalse();
            response.Error.Code.Should().Be("missing_value");
        }

        [Fact]
        public async Task ShouldReturnErrorIfLongUrlIsNotHttp()
        {
            var request = CreateAddUrlRequest(createdBy: string.Empty);

            var response = await _urlService.CreateShortenUrlAsync(request, default);

            response.Succeeded.Should().BeFalse();
            response.Error.Code.Should().Be("missing_value");
        }

        private static CreateUrlRequest CreateAddUrlRequest(string createdBy = "gui@guiferreira.me")
        {
            return new CreateUrlRequest(
                new Uri("https://qtmviet.com"),
                createdBy);
        }
    }
}
