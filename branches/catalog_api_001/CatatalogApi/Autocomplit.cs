using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Windows.Forms;

namespace CatalogApi
{
    public class Autocomplit
    {
        public static bool ClickEPCSubmit(HtmlElement htmlElement)
        {
            htmlElement.SetAttribute("target", "_self");
            if (htmlElement.Document != null) {
                HtmlElementCollection elementsByTagName =
                    htmlElement.Document.GetElementsByTagName("input");
                foreach (HtmlElement current in from HtmlElement element in elementsByTagName
                    where element.GetAttribute("value")
                        .Equals("EPC", StringComparison.InvariantCultureIgnoreCase)
                    select element) {
                    current.InvokeMember("click");

                    return true;
                }
            } else
                ;

            return false;
        }

        public static bool TypeCredentials(HtmlDocument document, HtmlElement element, params string[] credentionals)
        {
            HtmlElement elementById;

            element.SetAttribute("target", "_self"); //???
            if (element.Document != null) {
                elementById = document.GetElementsByTagName("input")
                    .Cast<HtmlElement>()
                    .First<HtmlElement>(x => x.GetAttribute("name").Equals(credentionals[0]));
                if (elementById != null) {
                    elementById.SetAttribute("value", credentionals[1]);
                } else
                    return false;

                elementById = document.GetElementsByTagName("input")
                    .Cast<HtmlElement>()
                    .First<HtmlElement>(x => x.GetAttribute("name").Equals(credentionals[2]));
                if (elementById != null) {
                    elementById.SetAttribute("value", credentionals[3]);
                } else
                    return false;

                elementById = document.GetElementsByTagName("input")
                    .Cast<HtmlElement>()
                    .First<HtmlElement>(x => x.GetAttribute("type").Equals("submit"));
                if (elementById != null) {
                    elementById.InvokeMember("click");
                } else
                    return false;
            } else
                ;

            return true;
        }
    }
}
