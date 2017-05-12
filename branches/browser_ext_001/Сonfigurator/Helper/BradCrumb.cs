using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Сonfigurator.DataContext;

namespace Сonfigurator.Helper
{
    static class BradCrumb
    {
        public static string CreatePathForGroupBox(int groupId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var group = ae.Group.FirstOrDefault(t => t.GroupId == groupId);
                return string.Format("<a href='/Group/'>{0}</a> >> <a href='/GroupBox/?groupId={1}'>{2}</a>", "Группы", group.GroupId, group.Name);
            }
        }
        public static string CreatePathForBrand(int groupboxId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var groupbox = ae.GroupBox.FirstOrDefault(t => t.GroupBoxId == groupboxId);
                return string.Format("{0} >> <a href='/Brand/?groupboxId={1}'>{2}</a>",CreatePathForGroupBox(groupbox.GroupId),groupbox.GroupBoxId,groupbox.Title);
            }
        }
        public static string CreatePathForProvider(int brandId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var brand = ae.Brand.Include("GroupBox").FirstOrDefault(t => t.BrandId == brandId);
                return string.Format("{0}>> <a href='/Provider/?brandId={1}'>{2}</a> ", CreatePathForBrand(brand.GroupBoxId), brand.BrandId, brand.NameAndFolder);
            }
        }
        public static string CreatePathForCommandFile(int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var provider = ae.Provider.Include("Brand").FirstOrDefault(t => t.ProviderId == providerId);
                return string.Format("{0} >> <a href='/CommandFile/?providerId={1}'>{2}</a>", CreatePathForProvider(provider.Brand.BrandId), provider.ProviderId, provider.Title);
            }
        }
        public static string CreatePathForProviderFile(int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var provider = ae.Provider.Include("Brand").FirstOrDefault(t => t.ProviderId == providerId);
                return string.Format("{0} >> <a href='/ProviderFile/?providerId={1}'>{2}</a>", CreatePathForProvider(provider.Brand.BrandId), provider.ProviderId, provider.Title);
            }
        }
        public static string CreatePathForProviderAccount(int providerId)
        {
            using (var ae = new AvtoritetEntities())
            {
                var provider= ae.Provider.Include("Brand").FirstOrDefault(t => t.ProviderId == providerId);
                return string.Format("{0} >> <a href='/ProviderAccount/?providerId={1}'>{2}</a>",CreatePathForProvider(provider.Brand.BrandId),provider.ProviderId,provider.Title);
            }
        }
    }
}
