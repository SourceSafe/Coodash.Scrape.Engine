using System;
using System.Linq;
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Net;
using Coodash.Scrape.Common;

namespace Coodash.Scrape.Scrapers
{
    public class AFMENewsScrapePage : IWorkerItem
    {
        private string _pageURL;
        private ScrapeEngine _engine;
        HtmlWeb _web;
        
        public AFMENewsScrapePage(string pageURL, ScrapeEngine engine)
        {
             _web=  new HtmlWeb();
            _pageURL = pageURL;
            _engine = engine;
        
        }

        public void Work()
        {            
            try
            {
                
                var node = _web.Load(_pageURL).DocumentNode;
                var summaryNodes = node.SelectNodes("//div[@class='row no-gutters']//div[@class='col-12 px-3']")
                    .Select(item=>item.SelectSingleNode(".//a[@class='afme-article__link']")).ToList();

                foreach (var summaryNode in summaryNodes)
                {                    
                    ScrapedArticleArgs parsedArticle = new ScrapedArticleArgs() {ArticleType="News", ArticleTags = "None", Publisher = EnumPublisher.AFME, Body = "n/a", DownloadFiles = "n/a" };
                    parsedArticle.ArticleUrl = "https://www.afme.eu" + summaryNode.Attributes["href"].Value;
                    _engine.AddToQueue(new AFMENewsScrapeArticle(parsedArticle, _engine));                  
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

           
        }
    }
}
 