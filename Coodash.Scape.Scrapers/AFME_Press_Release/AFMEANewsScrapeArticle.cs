
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System;
using System.Linq;
using Coodash.Scrape.Common;
using System.Diagnostics;


namespace Coodash.Scrape.Scrapers
{
    public class AFMENewsScrapeArticle : IWorkerItem
    {
        private ScrapeEngine _engine;
        private ScrapedArticleArgs _newsArticle;        
        private HtmlWeb _web;
        public AFMENewsScrapeArticle(ScrapedArticleArgs newsArticle, ScrapeEngine engine)
        {
            _web = new HtmlWeb();
            _newsArticle = newsArticle;
            _engine = engine;
        }

        public void Work()
        {            
            try
            {
                var doc = _web.Load(_newsArticle.ArticleUrl).DocumentNode;

                               
                _newsArticle.Header = doc.SelectSingleNode("//div[@class='article-header__title']").InnerText.Replace("\n", "").Trim();                
                _newsArticle.PublishDate = doc.SelectSingleNode("//div[@class='article-header__date']").InnerText.Replace("\n", "").Trim();                
                _newsArticle.LatestUpdateDate = doc.SelectSingleNode("//div[@class='article-header__date']").InnerText.Replace("\n", "").Trim();                


                
                if(doc.SelectSingleNode("//a[@class='article-header__download-link']")!=null)
                {
                    _newsArticle.ArticleFormat = "pdf";
                    _newsArticle.DownloadFiles = "https://www.afme.eu" + doc.SelectSingleNode("//a[@class='article-header__download-link']").Attributes["href"].Value;
                }
                else
                {
                    _newsArticle.ArticleFormat = "web";
                }                                                
                _newsArticle.Body = doc.SelectSingleNode("//div[@class='article-content clearfix']").InnerText.Replace("\n", "").Trim();
                
                _engine.OnNewsArticleParsedDelegate(_newsArticle);
                     
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
