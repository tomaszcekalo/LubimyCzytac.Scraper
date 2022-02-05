using AngleSharp;
using AngleSharp.Dom;
using System.Globalization;

namespace LubimyCzytac.Scraper
{
    public interface ILubimyCzytacScraper
    {
        Task<UserProfile> ScrapeUserProfileAsync(long userId, CancellationToken cancellationToken = default);

        Task<UserProfile> ScrapeUserProfileAsync(string url, CancellationToken cancellationToken = default);

        Task<List<UserReview>> ScrapeUserReviewsAsync(string url, CancellationToken CancellationToken = default);

        Task<List<UserReview>> ScrapeUserReviewsAsync(long userId, CancellationToken CancellationToken = default);

        Task<BookProfile> ScrapeBookAsync(string url, CancellationToken cancellationToken = default);

        Task<List<KatalogBook>> ScrapeKatalogAsync(int page, CancellationToken cancellationToken = default);
    }

    public class LubimyCzytacScraper : ILubimyCzytacScraper
    {
        private IConfiguration _config;
        private IBrowsingContext _context;
        private CultureInfo _cultureInfo;

        public LubimyCzytacScraper()
        {
            _config = Configuration.Default.WithDefaultLoader();
            _context = BrowsingContext.New(_config);
            _cultureInfo = new CultureInfo("pl-PL");
        }

        public async Task<List<KatalogBook>> ScrapeKatalogAsync(int page, CancellationToken cancellationToken = default)
        {
            var url = string.Format(Consts.KatalogUrlTemplate, page);
            var document = await _context.OpenAsync(url, cancellationToken);
            if (document.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Forbidden");
            var books = ParseKatalogBooks(document);
            return books;
        }

        private List<KatalogBook> ParseKatalogBooks(IDocument document)
        {
            var books = document.QuerySelectorAll("#booksFilteredList .authorAllBooks__single")
                .Select(x => ParseBookInKatalog(x))
                .ToList();
            return books;
        }

        private KatalogBook ParseBookInKatalog(IElement x)
        {
            var imgWrap = x.QuerySelector("form.authorAllBooks__singleImgWrap");
            var name = x.QuerySelector("a.authorAllBooks__singleTextTitle");
            var result = new KatalogBook
            {
                Url = imgWrap?.Attributes["action"]?.Value,
                Image = imgWrap?.QuerySelector("img")?.Attributes["src"]?.Value,
                Name = name?.TextContent.Trim()
            };
            return result;
        }

        public async Task<UserProfile> ScrapeUserProfileAsync(string url, CancellationToken cancellationToken = default)
        {
            var document = await _context.OpenAsync(url, cancellationToken);
            if (document.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Forbidden");
            var profile = ParseProfile(document);
            return profile;
        }

        public async Task<UserProfile> ScrapeUserProfileAsync(long userId, CancellationToken cancellationToken = default)
        {
            var url = Consts.LubimyCzytacUserProfileUrl + userId.ToString();
            return await ScrapeUserProfileAsync(url, cancellationToken);
        }

        public async Task<List<UserReview>> ScrapeUserReviewsAsync(string url, CancellationToken CancellationToken = default)
        {
            var document = await _context.OpenAsync(url, CancellationToken);
            if (document.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Forbidden");
            var reviews = ParseReviewRows(document);
            return reviews;
        }

        public async Task<List<UserReview>> ScrapeUserReviewsAsync(long userId, CancellationToken CancellationToken = default)
        {
            var url = string.Format(Consts.LubimyCzytacUserReviewsUrlTemplate, userId);
            var reviews = await ScrapeUserReviewsAsync(url);
            return reviews;
        }

        private UserProfile ParseProfile(IDocument doc)
        {
            var reviews = ParseReviewRows(doc);
            var moreReviewsUrl = doc
                .QuerySelector("a.btn-primary")
                ?.Attributes["href"]
                ?.Value;
            var userInfo = doc.QuerySelectorAll(".dashBoardAccount__introSecond__avatarInfo span")
                .Select(x => x.TextContent.Trim())
                .ToArray();
            string? city = null, sex = null;
            if (userInfo.Any())
            {
                city = userInfo[0];
                sex = userInfo.Last();
            }
            return new UserProfile()
            {
                Reviews = reviews,
                MoreReviewsUrl = moreReviewsUrl,
                City = city,
                Sex = sex
            };
        }

        private List<UserReview> ParseReviewRows(IParentNode parentNode)
        {
            var result = parentNode.QuerySelectorAll("#listOfReviews div.row")
                .Select(x => ParseUserReviewRow(x))
                .Where(x => x != null)
                .ToList();
            return result;
        }

        private UserReview? ParseUserReviewRow(IElement x)
        {
            var dateString = x.QuerySelector("span.comment-cloud__date")?.TextContent.Trim();
            if (String.IsNullOrEmpty(dateString))
                return null;
            var ratingValueString = x.QuerySelector(".rating-value .big-number")?.TextContent.Trim();
            if (String.IsNullOrEmpty(ratingValueString))
                return null;
            if (DateOnly.TryParse(dateString, _cultureInfo, DateTimeStyles.None, out var date)
                && int.TryParse(ratingValueString, out int ratingValue))
            {
                var result = new UserReview()
                {
                    Date = date,
                    RatingValue = ratingValue,
                    BookUrl = x.QuerySelector("a.book-url")?.Attributes["href"]?.Value,
                    BookTitle = x.QuerySelector(".small-book-title")?.Attributes["title"]?.Value,
                    BookImgSrc = x.QuerySelector(".book-url img")?.Attributes["src"]?.Value,
                    AuthorUrl = x.QuerySelector("a.author-url")?.Attributes["href"]?.Value,
                    AuthorName = x.QuerySelector(".small-book-author")?.Attributes["title"]?.Value
                };

                return result;
            }
            return null;
        }

        public async Task<BookProfile> ScrapeBookAsync(string url, CancellationToken cancellationToken = default)
        {
            var document = await _context.OpenAsync(url, cancellationToken);
            if (document.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Forbidden");
            var profile = ParseBook(document);
            return profile;
        }

        private BookProfile ParseBook(IDocument document)
        {
            var result = new BookProfile()
            {
                Reviews = document.QuerySelectorAll("#reviewsList .row")
                    .Select(x => ParseBookReviewRow(x))
                    .Where(x => x != null)
                    .ToList()
            };
            return result;
        }

        private BookReview? ParseBookReviewRow(IElement x)
        {
            var dateString = x.QuerySelector("span.comment-cloud__date")?.TextContent.Trim();
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }
            var ratingValueString = x.QuerySelector(".rating-value .big-number")?.TextContent.Trim();
            if (string.IsNullOrEmpty(ratingValueString))
            {
                return null;
            }
            if (DateOnly.TryParse(dateString, _cultureInfo, DateTimeStyles.None, out var date)
                && int.TryParse(ratingValueString, out int ratingValue))
            {
                var result = new BookReview()
                {
                    Date = date,
                    RatingValue = ratingValue,
                    UserProfileUrl = x.QuerySelector(".comment-cloud__postInfo a")?.Attributes["href"]?.Value,
                    UserName = x.QuerySelector(".comment-cloud__postInfo a span")?.TextContent.Trim(),
                    UserAvatar = x.QuerySelector("img.avatar")?.Attributes["src"]?.Value
                };
                return result;
            }
            return null;
        }
    }
}