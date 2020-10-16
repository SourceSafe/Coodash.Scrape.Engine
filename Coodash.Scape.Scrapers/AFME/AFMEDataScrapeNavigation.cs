using Coodash.Scrape.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coodash.Scrape.Scrapers
{
    public class AFMEDataScrapeNavigation : IWorkerItem
    {
        private string _url;
        private int _pageScrapeCount;
        private int articlesPerPage = 10;
        private ScrapeEngine _engine;
        string _articleType;


        public AFMEDataScrapeNavigation(string url, int pageScapeCount, ScrapeEngine engine)
        {
            _url = url;
            _pageScrapeCount = pageScapeCount;
            _engine = engine;             
    }

        public void Work()
        {
            try
            {
                _engine.AddToQueue(new AFMEDataScrapePage(_url, _engine));                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
