using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;
using Ninject;
using PushNotification.DataContext;
using PushNotification.Repository;

namespace PushNotification.Controllers
{
    public class NewsController : ApiController
    {
        [Inject]
        public INewsRepository NewsRepository { get; set; }

        public IEnumerable<NewsLog> Get()
        {
            var newsLogs = new Collection<NewsLog>();

            try
            {
                foreach (var newsLog in NewsRepository.NewsLogs)
                {
                    newsLogs.Add(new NewsLog
                    {
                        Id = newsLog.Id,
                        Title = newsLog.Title,
                        Message = newsLog.Message,
                        PostTime = newsLog.PostTime
                    });
                }

                return newsLogs;
            }
            catch (Exception ex)
            {
                newsLogs.Add(new NewsLog
                {
                    Title = "Проблема с отображением новостей",
                    Message = ex.Message,
                    PostTime = DateTime.Now
                });

                return newsLogs;
            }
        }
    }
}