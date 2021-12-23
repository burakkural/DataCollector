using HtmlAgilityPack;
using Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NewsCollector
{
    public class EnsonhaberCom
    {
        private readonly string URL = "https://www.ensonhaber.com";
        private readonly string Endpoint = "/son-dakika/";
        private readonly int NewsCount = 18;
        private readonly int PageCount = 5;
        private ILogger _logger;

        public EnsonhaberCom(ILogger logger)
        {
            _logger = logger;
        }    
        public List<News> Collect()
        {
            List<News> newsList = new List<News>();         
           
            for (int page = 0; page <= PageCount; page++)
            {
                try
                {

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HtmlWeb web = new HtmlWeb { OverrideEncoding = Encoding.UTF8 };
                    HtmlDocument doc = web.Load(URL + Endpoint + page);

                    for (int j = 1; j <= NewsCount; j++)
                    {
                        try
                        {
                            string Title = "";
                            string Link = "";
                            string ImageLink = "";
                            string Description = "";

                            if (page == 0)
                            {
                                Title = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/section/div/div/a[{j}]/div/h4").InnerText;
                                Link = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/section/div/div/a[{j}]").GetAttributeValue("href", "");
                                ImageLink = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/section/div/div/a[{j}]/figure/img").GetAttributeValue("data-src", "");
                            }
                            else
                            {
                                Title = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/section/div/div/div[{page + 3}]/a[{j}]/div/h4").InnerText;
                                Link = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/section/div/div/div[{page + 3}]/a[{j}]").GetAttributeValue("href", "");
                                ImageLink = doc.DocumentNode.SelectSingleNode($"/html/body/div[2]/section/div/div/div[{page + 3}]/a[{j}]/figure/img").GetAttributeValue("data-src", "");
                            }
                            
                            News newsObj = new News
                            {
                                Title = Title,
                                Description = Description,
                                Url = URL + Link,
                                ImageUrl = ImageLink,
                                Source = NEWS_SOURCE.ENSONHABERCOM
                            };

#if DEBUG
                            WriteConsole(newsObj);
#endif
                            newsList.Add(newsObj);
                        }
                        catch (Exception)
                        {
                            _logger.Error($" Ensonhabercom {j}. satirinda haber yok incele ");
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return newsList;

        }

        private void WriteConsole(News obj)
        {
            Console.WriteLine(obj.Title);
            Console.WriteLine(obj.Description);
            Console.WriteLine(obj.Url);
            Console.WriteLine(obj.ImageUrl);
            Console.WriteLine(new string('*', 30));
        }
    }
}

