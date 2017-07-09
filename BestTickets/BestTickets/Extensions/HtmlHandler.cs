using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BestTickets.Extensions
{
    public class HtmlHandler
    {
        public static async Task<string> ReadHtmlPage(string url)
        {
            string siteMarkup = null;
            byte[] pageContent = null;
            try
            {
                pageContent = await new WebClient().DownloadDataTaskAsync(url);
                siteMarkup = Encoding.UTF8.GetString(pageContent);
            }
            catch (WebException exc)
            {
                if (exc.Status == WebExceptionStatus.ConnectFailure)
                    siteMarkup = "Service don't work yet.";
            }
            return siteMarkup;
        }

        public static HtmlNode LoadHtmlRootElement(string siteContent)
        {
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(siteContent);
            return html.DocumentNode;
        }

        public static HtmlNode GetElementById(HtmlNode node, string id)
        {
            return node.Descendants().Where(x => (x.Attributes["id"] != null && x.Attributes["id"].Value == id)).FirstOrDefault();
        }

        public static IEnumerable<string> GetElementValueByTag(HtmlNode node, string className)
        {
            return node.Descendants().Where(x => x.Name == className).Select(x => x.InnerText);   
        }

        public static IEnumerable<HtmlNode> GetElementByClass(HtmlNode node, string className)
        {
            return node.Descendants().Where(x => (x.Attributes["class"] != null && x.Attributes["class"].Value == className));
        }

        public static string GetFirstElementValueByClass(HtmlNode node, string className)
        {
            var value = node.Descendants().Where(x => (x.Attributes["class"] != null && x.Attributes["class"].Value == className)).FirstOrDefault();
            return value == null ? string.Empty : value.InnerText;
        }

        public static string GetLastElementValueByClass(HtmlNode node, string className)
        {
            var value =  node.Descendants().Where(x => (x.Attributes["class"] != null && x.Attributes["class"].Value == className)).LastOrDefault();
            return value == null ? string.Empty : value.InnerText;
        }

        public static string GetElementValueByClass(HtmlNode node, string className)
        {
            return node.Descendants().Where(x => (x.Attributes["class"] != null && x.Attributes["class"].Value == className))
                .Select(x => x.InnerText).Aggregate("", (x, y) => x += y);
        }
        
    }
}