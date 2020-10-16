
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System;
using System.Linq;
using Coodash.Scrape.Common;
using System.Diagnostics;


namespace Coodash.Scrape.Scrapers
{
    public class FCAScrapeArticle : IWorkerItem
    {
        private ScrapeEngine _engine;        
        private HtmlWeb _web;
        private ScrapedArticleArgs _newsArticle;
        public FCAScrapeArticle(ScrapedArticleArgs newsArticle, ScrapeEngine engine)
        {
            _web = new HtmlWeb();
            _newsArticle = newsArticle;            
            _engine = engine;
        }

        public void Work()
        {            
            try
            {                
                HtmlNode article = _web.Load(_newsArticle.ArticleUrl).DocumentNode.SelectSingleNode("//div[@role='main']");
                if (article != null)
                {                   
                    _newsArticle.Body= string.Join(Environment.NewLine, article.SelectNodes(".//p |.//h2 | .//h3").Select(item => item.InnerText));
                    _engine.OnNewsArticleParsedDelegate(_newsArticle);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
