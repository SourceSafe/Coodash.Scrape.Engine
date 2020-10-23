using Coodash.Scrape.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coodash.Scrape.Scrapers
{
    public class ESMAScrapePublicationNavigation : IWorkerItem
    {
        private string _url;
        private int _pageScrapeCount;
        private int articlesPerPage = 10;
        private ScrapeEngine _engine;
        private string _pageFormat = "{0}?page={1}";

        public ESMAScrapePublicationNavigation(string url, int pageScapeCount, ScrapeEngine engine)
        {
            _url = url;
            _pageScrapeCount = pageScapeCount;
            _engine = engine;
        }

        public void Work()
        {
            try
            {
                //Generate a ESMAScrapePage for each page needed
                for (int queryParam = 0; queryParam < articlesPerPage; queryParam++)
                    _engine.AddToQueue(new ESMAPublicationScrapeSummary(string.Format(_pageFormat, _url, queryParam), _engine));
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
