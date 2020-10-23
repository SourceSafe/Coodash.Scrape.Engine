using System;
using System.Linq;
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Net;
using Coodash.Scrape.Common;

namespace Coodash.Scrape.Scrapers
{
    public class ESMAPublicationScrapeSummary : IWorkerItem
    {
        private const int Index_Date = 0;
        private const int Index_Summary = 1;
        private const int Index_Title = 2;
        private const int Index_Tag = 3;
        private const int Index_Type = 4;
        private string _pageURL;
        private ScrapeEngine _engine;
        HtmlWeb _web;
        public ESMAPublicationScrapeSummary(string pageURL, ScrapeEngine engine)
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
                    .SelectNodes(".//tr").ToList();

                foreach(var summary in summaryNodes)
                {
                    ScrapedArticleArgs parsedArticle = new ScrapedArticleArgs() { Publisher = EnumPublisher.ESMA,  Body = "n/a", DownloadFiles = "n/a" };
                    var s = summary.SelectNodes(".//td");
                    parsedArticle.PublishDate = s[Index_Date].InnerText;
                    parsedArticle.LatestUpdateDate = parsedArticle.PublishDate;
                    parsedArticle.Summary = "Article Reference: " + s[Index_Summary].InnerText;
                    parsedArticle.Header = s[Index_Title].SelectSingleNode(".//a").InnerText;
                    parsedArticle.ArticleUrl = s[Index_Title].SelectSingleNode(".//a").Attributes["href"].Value;
                    parsedArticle.ArticleFormat = "pdf";
                    parsedArticle.ArticleTags = s[Index_Tag].InnerText;
                    parsedArticle.ArticleType = s[Index_Type].InnerText;
                    _engine.OnNewsArticleParsedDelegate(parsedArticle);

                }             
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

           
        }
    }
}
 