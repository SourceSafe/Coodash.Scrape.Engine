
using Coodash.Scrape.Engine;
using HtmlAgilityPack;
using System;
using System.Linq;
using Coodash.Scrape.Common;
using System.Diagnostics;


namespace Coodash.Scrape.Scrapers
{
    public class AFMEDataScrapeArticle : IWorkerItem
    {

        //Test git
        private ScrapeEngine _engine;
        private ScrapedArticleArgs _newsArticle;        
        private HtmlWeb _web;
        public AFMEDataScrapeArticle(ScrapedArticleArgs newsArticle, ScrapeEngine engine)
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
                var downLoadLinks = doc.SelectNodes("//a[@class='article-header__download-link']").ToList();
                if(downLoadLinks!=null)
                {
                    _newsArticle.DownloadFiles = string.Join("|", downLoadLinks.Select(item => item.Attributes["href"].Value.Contains("www")? item.Attributes["href"].Value: "https://www.afme.eu" + item.Attributes["href"].Value));

                }


                var content = doc.SelectNodes("//div[@class='article-content']//li");
                if(content != null)
                {
                    _newsArticle.Body = string.Join(Environment.NewLine, content.Select(txt => txt.InnerText.Trim()));
                }

       
                _engine.OnNewsArticleParsedDelegate(_newsArticle);
                     
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
