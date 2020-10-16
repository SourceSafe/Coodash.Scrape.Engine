using System;
using System.Linq;
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Net;
using Coodash.Scrape.Common;

namespace Coodash.Scrape.Scrapers
{
    public class ESMANewsScrapePage : IWorkerItem
    {
        private string _pageURL;
        private ScrapeEngine _engine;
        HtmlWeb _web;
        public ESMANewsScrapePage(string pageURL, ScrapeEngine engine)
        {
             _web=  new HtmlWeb();
            _pageURL = pageURL;
            _engine = engine;
        }

        public void Work()
        {            
            try
            {
                
                var summaryNodes = _web.Load(_pageURL).DocumentNode
                    .SelectSingleNode("//tbody")
                    .SelectNodes(".//td").ToList();

                foreach(var summary in summaryNodes)
                {
                    ScrapedArticleArgs parsedArticle = new ScrapedArticleArgs() { Publisher = EnumPublisher.ESMA, ArticleFormat="Web Page",  Body = "n/a", DownloadFiles = "n/a" };

                    var articleTagsNode = summary.SelectNodes(".//div[@class='section_link']")?.Select(item => item.InnerText).ToList();
                    if (articleTagsNode != null)
                    {
                        parsedArticle.ArticleTags = string.Join(":", articleTagsNode);
                    }                    
                    parsedArticle.Header = summary.SelectSingleNode("..//a").InnerText;
                    parsedArticle.ArticleUrl = $"https://www.esma.europa.eu{summary.SelectSingleNode(".//a").Attributes["href"].Value}" ;
                    parsedArticle.Summary = summary.SelectSingleNode(".//p").InnerText;
                    parsedArticle.PublishDate = summary.SelectSingleNode(".//div[@class='field field-type-ds']").InnerText.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                    parsedArticle.LatestUpdateDate = parsedArticle.PublishDate;
                    parsedArticle.DownloadFiles = summary.SelectSingleNode(".//div[@class='file']")?.SelectSingleNode(".//a").Attributes["href"].Value;

                    if(parsedArticle.DownloadFiles != null)                    
                    {
                        parsedArticle.ArticleFormat += "|Download";
                    }
                    else
                    {
                        parsedArticle.DownloadFiles = "n/a";
                    }

                    _engine.AddToQueue(new ESMANewsScrapeArticle(parsedArticle, _engine));
                }
             
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

           
        }
    }
}
 