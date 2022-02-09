using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LubimyCzytac.Scraper
{
    public class BookProfile
    {
        public List<BookReview> Reviews { get; set; }
        public string? BookTitle { get; internal set; }
        public string? BookImgSrc { get; internal set; }
        public string? AuthorUrl { get; internal set; }
        public string? AuthorName { get; internal set; }
    }
}