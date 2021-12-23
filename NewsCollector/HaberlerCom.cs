using HtmlAgilityPack;
using Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NewsCollector
{
    public  class HaberlerCom
    {
        private readonly string URL = "https://www.haberler.com";
        private readonly string Endpoint = "/son-dakika";
        private readonly int NewsCount = 50;
        private ILogger _logger;
        public HaberlerCom(ILogger logger)
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
                    var Title = doc.DocumentNode.SelectSingleNode($"/html/body/div[1]/div[4]/div[1]/div/div[{i}]/a/div[2]/span").InnerText;
                    var Description = doc.DocumentNode.SelectSingleNode($"/html/body/div[1]/div[4]/div[1]/div/div[{i}]/a/div[2]/p").InnerText;
                    var Link = doc.DocumentNode.SelectSingleNode($"/html/body/div[1]/div[4]/div[1]/div/div[{i}]/a").GetAttributeValue("href","");
                    var ImageLink = doc.DocumentNode.SelectSingleNode($"/html/body/div[1]/div[4]/div[1]/div/div[{i}]/a/div[1]/img").GetAttributeValue("data-src", "");

                    News newsObj = new News
                    {
                        Title = Title,
                        Description = Description,
                        Url = URL + Link,
                        ImageUrl = ImageLink,
                        Source = NEWS_SOURCE.HABERLERCOM
                    };

#if DEBUG
                    Utilities.WriteConsole(newsObj);
#endif
                    newsList.Add(newsObj);
                }
                catch (Exception)
                {
                    _logger.Error($" HaberlerCom {i}. satirinda haber yok incele "); 
                    continue;
                }
            }

            return newsList;

        }
    }
}
