using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coodash.Scrape.Common
{
    public delegate void NewsArticleParsedDelegate(ScrapedArticleArgs args);

    public class ScrapedArticleArgs : EventArgs
    {
        public string ArticleUrl;
        public string ArticleType;
        public string ArticleFormat;
        public string ArticleTags;        
        public EnumPublisher Publisher;                        
        public String PublishDate;
        public String LatestUpdateDate;
        public String Header;
        public string Summary;
        public String Body;
        public string DownloadFiles;



        public ScrapedArticleArgs()
        {

        }
        

            public ScrapedArticleArgs(string URL, EnumPublisher publisher, string articleType, String publishDate, String latestUpdateDate, String header, String body)
            {
            ArticleUrl = URL;
            Publisher = publisher;
            ArticleTags = articleType;
            PublishDate = publishDate;
            LatestUpdateDate = latestUpdateDate;
            Header = header;
            Body = body;
                }

        public override string ToString()
        {            
            return $"URL: {ArticleUrl}{Environment.NewLine}Article Format:{ArticleFormat}" +
                $"{Environment.NewLine}Publisher: {Publisher}" +
                $"{Environment.NewLine} Article Type: {ArticleType}" +
                $"{Environment.NewLine} Title: {Header}" +
                $"{Environment.NewLine}Summary:{Summary}" +
                $"{Environment.NewLine}Tags: {ArticleTags}" +
                $"{Environment.NewLine}Publish Date: {PublishDate}" +
                $"{Environment.NewLine}Lastest Update: {LatestUpdateDate}" +
                $"{Environment.NewLine}Body: {Body}" +
                $"{Environment.NewLine}Download File: {DownloadFiles}" +
                $"{Environment.NewLine}{Environment.NewLine}" +
                $"{Environment.NewLine}{Environment.NewLine}";
        }
    }
}
