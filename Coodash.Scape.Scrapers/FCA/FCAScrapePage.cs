using System;
using System.Linq;
using System.Linq.Expressions;
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System.Diagnostics;
using Coodash.Scrape.Common;
using System.Collections.Generic;

namespace Coodash.Scrape.Scrapers
{
    public class FCAScrapePage : IWorkerItem
    {
        private string _pageURL;
        private ScrapeEngine _engine;
        HtmlWeb _web;
        public FCAScrapePage(string pageURL, ScrapeEngine engine)
        {
             _web=  new HtmlWeb();
            _pageURL = pageURL;
            _engine = engine;
        }

        public void Work()
        {            
            try
            {                
                //Go through each news summary item and process                          
                var summaryListNode = _web.Load(_pageURL).DocumentNode.SelectSingleNode("//ol[@class='search-list']");
                foreach(var summaryItemNode in summaryListNode.SelectNodes(".//li"))
                {
                    ScrapedArticleArgs parsedArticle = new ScrapedArticleArgs() { Publisher = EnumPublisher.FCA, Body = "n/a" , DownloadFiles = "n/a"};
                    //Header
                    var linkNode = summaryItemNode.SelectSingleNode(".//a");
                    parsedArticle.Header = linkNode.InnerText.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                    //ArticleTags
                    parsedArticle.ArticleTags = summaryItemNode.SelectSingleNode(".//span[@class='meta-item type']")?.InnerText.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                    //Publish
                    parsedArticle.PublishDate = summaryItemNode.SelectSingleNode(".//span[@class='meta-item published-date']")?.InnerText.Split(':')[1].Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();                   
                    //LatestUpdateDate
                    parsedArticle.LatestUpdateDate = summaryItemNode.SelectSingleNode(".//span[@class='meta-item modified-date']")?.InnerText.Split(':')[1].Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                    if (parsedArticle.LatestUpdateDate == null)
                        parsedArticle.LatestUpdateDate = parsedArticle.PublishDate;

                    //Summary
                    parsedArticle.Summary = summaryItemNode.SelectSingleNode(".//div[@class='search-item__body']")?.InnerText.Replace("\t", String.Empty).Replace("\n", String.Empty).Trim();
                    //URL
                    parsedArticle.ArticleUrl = linkNode.Attributes["href"].Value;
                    
                    List<string> parts = parsedArticle.Header.Split(' ').ToList();
                    
                    if ((parts[parts.Count - 1].Contains("[")))
                    {
                        parsedArticle.ArticleFormat = parts[parts.Count - 1];
                        _engine.OnNewsArticleParsedDelegate(parsedArticle);
                    }
                    else 
                    {
                        parsedArticle.ArticleFormat = "Web Page";
                        _engine.AddToQueue(new FCAScrapeArticle(parsedArticle, _engine));
                    }                                        
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

           
        }
    }
}
 