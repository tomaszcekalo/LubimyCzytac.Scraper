using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LubimyCzytac.Scraper
{
    public class Consts
    {
        public const string LubimyCzytacBaseUrl = "https://lubimyczytac.pl";
        public const string LubimyCzytacUserProfileUrl = LubimyCzytacBaseUrl + "/profil/";
        public const string LubimyCzytacUserReviewsUrlTemplate = LubimyCzytacUserProfileUrl + "{0}/oficjalne-recenzje";

        public const string KatalogUrlTemplate = "https://lubimyczytac.pl/katalog?page={0}&catalogSortBy=ratings-desc";
    }
}