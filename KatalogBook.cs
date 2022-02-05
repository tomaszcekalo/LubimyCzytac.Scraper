using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LubimyCzytac.Scraper
{
    public class KatalogBook
    {
        public string? Url { get; internal set; }
        public string? Image { get; internal set; }
        public string? Name { get; internal set; }
    }
}