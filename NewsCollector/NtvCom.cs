using HtmlAgilityPack;
using Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NewsCollector
{
  public  class NtvCom
    {
        private readonly string URL = "https://www.ntv.com.tr";
        private readonly string Endpoint = "/son-dakika";
        private readonly int NewsCount = 50;
        private ILogger _logger;

        public NtvCom(ILogger logger)
        {
            _logger = logger;
        }

        public List<News> Collect()
        {
            List<News> newsList = new List<News>();

            HtmlWeb web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HtmlDocument doc = web.Load(URL + Endpoint);

            for (int i = 1; i <= NewsCount; i++)
            {
                try
                {
                    String contentXPath = $"/html/body/div[@class='common-container']/div/div[@class='container']/div[@class='ntv-content']/div/ul[@class='gallery-page-video-list-items']/li[{i}]/div/div[2]/p/a";
                    String imageXPath = $"/html/body/div[@class='common-container']/div/div[@class='container']/div[@class='ntv-content']/div/ul[@class='gallery-page-video-list-items']/li[{i}]/div/div[@class='card-img-wrapper lazy-loading-img']/a/picture/img";
                    
                    var Title = doc.DocumentNode.SelectSingleNode(contentXPath).InnerText.Trim().Replace(System.Environment.NewLine, string.Empty);
                    var Description = string.Empty;
                    var Link = doc.DocumentNode.SelectSingleNode(contentXPath).GetAttributeValue("href", "");
                    var ImageLink = doc.DocumentNode.SelectSingleNode(imageXPath).GetAttributeValue("data-src", "");

                    News newsObj = new News
                    {
                        Title = Title,
                        Description = Description,
                        Url = URL + Link,
                        ImageUrl = ImageLink,
                        Source = NEWS_SOURCE.NTVCOM
                    };

#if DEBUG
                    Utilities.WriteConsole(newsObj);
#endif
                    newsList.Add(newsObj);
                }
                catch (Exception)
                {
                    _logger.Error($" NtvCom {i}. satirinda haber yok incele ");
                    continue;
                }
            }

            return newsList;

        }
    }
}
