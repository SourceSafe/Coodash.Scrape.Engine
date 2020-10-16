using Coodash.Scrape.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coodash.Scrape.Scrapers
{
    public class FCAScrapeNavigation: IWorkerItem
    {
        private string _url;
        private int _pageScrapeCount;
        private int articlesPerPage = 10;
        private ScrapeEngine _engine;
        private string _pageFormat = "{0}?start={1}";

        public FCAScrapeNavigation(string url, int pageScapeCount, ScrapeEngine engine)
        {
            _url = url;
            _pageScrapeCount = pageScapeCount;
            _engine = engine;
        }

        public void Work()
        {
            try
            {
                //Generate a FCAScrapePage for each page
                for (int queryParam = 1; queryParam < _pageScrapeCount * articlesPerPage; queryParam += articlesPerPage)
                    _engine.AddToQueue(new FCAScrapePage(string.Format(_pageFormat, _url, queryParam), _engine));
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
