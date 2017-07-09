using System.IO;
using System.Net;
using System.Text;
namespace BestTickets.Extensions
{
    public class CustomRequest
    {

        public static string SendRequest(string url, string method, string content = null, string referer = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var data = Encoding.ASCII.GetBytes(content);
            if (request != null)
            {
                request = SetRequestHeader(request, method, referer, content);
                using (var requestStream = request.GetRequestStream())
                    requestStream.Write(data, 0, data.Length);

                var response = (HttpWebResponse)request.GetResponse();
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            return null;
        }

        private static HttpWebRequest SetRequestHeader(HttpWebRequest request, string method, string referer = null, string content = null)
        {
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Referer = referer;
            request.ContentLength = content.Length;
            return request;
        }


    }
}