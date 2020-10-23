using System;
using Coodash.Scrape.Common;
using Coodash.Scrape.Engine;
using Coodash.Scrape.Scrapers;
using System.Diagnostics;
using System.Net;

namespace Coodash.Scrape.Main
{
    class Program
    {
        private static int _threadCount = 1;
        private static int _pageCount = 10;
        private static string FCASearchResultsURL = "https://www.fca.org.uk/news/search-results";
        private static string FCAPublicationsURL = "https://www.fca.org.uk/publications/search-results";
        private static string ESMANewsURL = "https://www.esma.europa.eu/press-news/esma-news";
        private static string ESMAPublicationsURL = "https://www.esma.europa.eu/press-news/press-releases";
        private static string AFMEDataURL = "https://www.afme.eu/Reports/Data";
        private static string AFMEPublicationsURL = "https://www.afme.eu/Reports/Publications";
        private static string AFMENewsURL = " https://www.afme.eu/News/Press-Releases";
       
                        
        static void Main(string[] args)
        {
            //Add commnet
            ScrapeEngine _engine = new ScrapeEngine();
            _engine.NewsArticleParsed += _engine_NewsArticleParsed;
            _engine.Start(_threadCount);

            
            // using System.Net;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //Parse FCA  
            _engine.AddToQueue(new FCAScrapeNavigation(FCASearchResultsURL, _pageCount, _engine));
            _engine.AddToQueue(new FCAScrapeNavigation(FCAPublicationsURL, _pageCount, _engine));

            //Parse ESMA 
            _engine.AddToQueue(new ESMANewsScrapeNewsNavigation(ESMANewsURL, _pageCount, _engine));
            _engine.AddToQueue(new ESMAScrapePublicationNavigation(ESMAPublicationsURL, _pageCount, _engine));

            //Parse AFME 
            _engine.AddToQueue(new AFMEDataScrapeNavigation(AFMEDataURL, _pageCount, _engine));
            _engine.AddToQueue(new AFMEDataScrapeNavigation(AFMEPublicationsURL, _pageCount, _engine));
            
            _engine.AddToQueue(new AFMENewsScrapeNavigation(AFMENewsURL, _pageCount, _engine));

        }

        private static void _engine_NewsArticleParsed(ScrapedArticleArgs args)
        {        
            Debug.WriteLine(args);
        }
    }
}

