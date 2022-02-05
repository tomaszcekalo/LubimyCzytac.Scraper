using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LubimyCzytac.Scraper
{
    public class UserReview
    {
        public DateOnly Date { get; set; }
        public int RatingValue { get; set; }
        public string? BookUrl { get; set; }
        public string? BookTitle { get; set; }
        public string? AuthorUrl { get; set; }
        public string? AuthorName { get; set; }
        public string? BookImgSrc { get; set; }
    }
}