
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System;
using System.Linq;
using Coodash.Scrape.Common;
using System.Diagnostics;


namespace Coodash.Scrape.Scrapers
{
    public class ESMANewsScrapeArticle : IWorkerItem
    {
        private ScrapeEngine _engine;        
        private HtmlWeb _web;
        private ScrapedArticleArgs _article;
        public ESMANewsScrapeArticle(ScrapedArticleArgs article, ScrapeEngine engine)
        {
            _web = new HtmlWeb();
            _article = article;
            _engine = engine;
        }

        public void Work()
        {            
            try
            {                
                HtmlNode article = _web.Load(_article.ArticleUrl).DocumentNode.SelectSingleNode("//div[@id='esmapage_main-content']");
                if (article != null)
                {
                    _article.Body = string.Join(Environment.NewLine, article.SelectNodes(".//p |.//h2 | .//h3").Select(item => item.InnerText));
                    _engine.OnNewsArticleParsedDelegate(_article);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
