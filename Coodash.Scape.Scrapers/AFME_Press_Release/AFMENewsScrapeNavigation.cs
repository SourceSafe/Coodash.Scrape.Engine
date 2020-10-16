using Coodash.Scrape.Engine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coodash.Scrape.Scrapers
{
    public class AFMENewsScrapeNavigation : IWorkerItem
    {
        private string _url;                
        private ScrapeEngine _engine;
        
        public AFMENewsScrapeNavigation(string url, int pageScapeCount, ScrapeEngine engine)
        {
            _url = url;            
            _engine = engine;             
    }

        public void Work()
        {
            try
            {
                _engine.AddToQueue(new AFMENewsScrapePage(_url, _engine));                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
