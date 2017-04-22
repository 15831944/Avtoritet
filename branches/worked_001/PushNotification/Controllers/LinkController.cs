using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;
using Ninject;
using PushNotification.DataContext;
using PushNotification.Repository;

namespace PushNotification.Controllers
{
    public class LinkController : ApiController
    {
        [Inject]
        public ILinkRepository LinkRepository { get; set; }

        public IEnumerable<Link> Get()
        {
            var links = new Collection<Link>();

            try
            {
                foreach (var link in LinkRepository.Links)
                {
                    links.Add(new Link
                    {
                        Id = link.Id,
                        Url = link.Url
                    });
                }

                return links;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}