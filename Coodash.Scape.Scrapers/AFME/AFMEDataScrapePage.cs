using System;
using System.Linq;
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Net;
using Coodash.Scrape.Common;

namespace Coodash.Scrape.Scrapers
{
    public class AFMEDataScrapePage : IWorkerItem
    {
        private string _pageURL;
        private ScrapeEngine _engine;
        HtmlWeb _web;
        
        public AFMEDataScrapePage(string pageURL, ScrapeEngine engine)
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
                var summaryNodes = node.SelectNodes("//div[@class='template-main-container']/div[@class='row no-gutters']//div[@class='col-12 col-md-6 px-3']")
                    .Select(item=>item.SelectSingleNode("./div/a")).ToList();

                foreach (var summaryNode in summaryNodes)
                {
                    ScrapedArticleArgs parsedArticle = new ScrapedArticleArgs() {ArticleTags="None",   Publisher = EnumPublisher.AFME, Body = "n/a", DownloadFiles = "n/a" };

                    //URL
                    parsedArticle.ArticleUrl = "https://www.afme.eu" + summaryNode.Attributes["href"].Value;
                    //Article Format
                    parsedArticle.ArticleFormat = "pdf";

                    var nodeDetails = summaryNode.SelectNodes("./div//div");
                    //Header
                    parsedArticle.Header = nodeDetails[1].InnerText.Trim();
                    //Public and Modified Date
                    parsedArticle.LatestUpdateDate = parsedArticle.PublishDate = nodeDetails[2].InnerText.Trim();                   
                    //Summary
                    parsedArticle.Summary =  nodeDetails[3].InnerText.Replace("\n", "").Trim();

                    _engine.AddToQueue(new AFMEDataScrapeArticle(parsedArticle, _engine));

                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

           
        }
    }
}
 