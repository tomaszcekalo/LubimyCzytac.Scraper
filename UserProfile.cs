using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LubimyCzytac.Scraper
{
    public class UserProfile
    {
        public string City { get; set; }
        public string? MoreReviewsUrl { get; set; }
        public object Reviews { get; set; }
        public string Sex { get; set; }
    }
}