using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using UrlShortener.Models;
using UrlShortener.Repositories.ShortUrlRep;
using UrlShortener.Repositories.UnitOfWorkRep;
using UrlShortener.Services.Redirect;

namespace UrlShortener.Tests
{
    public class UrlRedirectServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IDatabase> _redisDbMock;
        private readonly Mock<IConnectionMultiplexer> _redisMultiplexerMock;
        private readonly UrlRedirectService _redirectService;

        public UrlRedirectServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _redisDbMock = new Mock<IDatabase>();
            _redisMultiplexerMock = new Mock<IConnectionMultiplexer>();

            _redisMultiplexerMock.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                                 .Returns(_redisDbMock.Object);

            _redirectService = new UrlRedirectService(
                _unitOfWorkMock.Object,
                _redisMultiplexerMock.Object,
                new Mock<ILogger<UrlRedirectService>>().Object);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldReturnUrlFromCache_WhenCacheHit()
        {
            var shortCode = "testCode";
            var expectedUrl = "https://google.com";

            _redisDbMock.Setup(db => db.StringGetAsync(shortCode, It.IsAny<CommandFlags>()))
                        .ReturnsAsync(new RedisValue(expectedUrl));

            var result = await _redirectService.GetOriginalUrlAsync(shortCode);

            result.Should().Be(expectedUrl);

            _unitOfWorkMock.Verify(uow => uow.ShortUrls.FindByShortCodeAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldReturnUrlFromDbAndSetCache_WhenCacheMiss()
        {
            var shortCode = "testCode";
            var expectedUrl = "https://google.com";
            var shortUrlFromDb = new ShortUrl { OriginalUrl = expectedUrl, ShortCode = shortCode };

            _redisDbMock.Setup(db => db.StringGetAsync(shortCode, It.IsAny<CommandFlags>()))
                        .ReturnsAsync(RedisValue.Null);

            var shortUrlRepoMock = new Mock<IShortUrlRepository>();
            shortUrlRepoMock.Setup(repo => repo.FindByShortCodeAsync(shortCode)).ReturnsAsync(shortUrlFromDb);
            _unitOfWorkMock.Setup(uow => uow.ShortUrls).Returns(shortUrlRepoMock.Object);

            var result = await _redirectService.GetOriginalUrlAsync(shortCode);

            result.Should().Be(expectedUrl);

            _redisDbMock.Verify(db => db.StringSetAsync(
                shortCode,
                expectedUrl,
                TimeSpan.FromHours(1),
                It.IsAny<bool>(),
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()
            ), Times.Once);
        }
    }
}