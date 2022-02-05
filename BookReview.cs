using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LubimyCzytac.Scraper
{
    public class BookReview
    {
        public DateOnly Date { get; set; }
        public int RatingValue { get; set; }
        public string? UserProfileUrl { get; set; }
        public string? UserName { get; set; }
        public string? UserAvatar { get; set; }
    }
}