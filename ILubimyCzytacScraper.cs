using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LubimyCzytac.Scraper
{
    public interface ILubimyCzytacScraper
    {
        Task<UserProfile> ScrapeUserProfileAsync(
            long userId,
            CancellationToken cancellationToken = default);

        Task<UserProfile> ScrapeUserProfileAsync(
            string url,
            CancellationToken cancellationToken = default);

        Task<UserProfileReviews> ScrapeUserReviewsAsync(
            string url,
            CancellationToken CancellationToken = default);

        Task<UserProfileReviews> ScrapeUserReviewsAsync(
            long userId,
            CancellationToken CancellationToken = default);

        Task<UserProfileReviews> ScrapeUserReviewsFromHtmlAsync(
            string html,
            CancellationToken CancellationToken = default);

        Task<BookProfile> ScrapeBookAsync(
            string url,
            CancellationToken cancellationToken = default);

        Task<BookProfile> ScrapeBookFromHtmlAsync(
            string html,
            CancellationToken cancellationToken = default);

        Task<List<KatalogBook>> ScrapeKatalogAsync(
            int page,
            CancellationToken cancellationToken = default);

        Task<List<KatalogBook>> ScrapeKatalogAsync(
            string htmlSourceCode,
            CancellationToken cancellationToken = default);

        Task<UserProfile> ScrapeUserProfileFromHtmlAsync(
            string html,
            CancellationToken cancellationToken = default);
    }
}