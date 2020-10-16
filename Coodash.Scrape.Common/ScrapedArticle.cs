using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coodash.Scrape.Common
{
    public class CooDashScrapedArticle
    {
        public String PublishDate;
        public String LatestUpdateDate;
        public String Header;
        public String Body;

        public CooDashScrapedArticle(String publishDate, String latestUpdateDate, String header, String body)
        {
            PublishDate = publishDate;
            LatestUpdateDate = latestUpdateDate;
            Header = header;
            Body = body;
        }

        public override string ToString()
        {
            return string.Format("Publish Date: {0} Lastest Update:{1} Title: {2} Body:{3}", PublishDate, LatestUpdateDate,Header, Body);
        }
    }
}
