using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace контакты
{
    public class HtmlPage
    {
        public HtmlDocument SiteContent { get; }
        public bool isForbidden;
        public bool isSuccessfully = true;
        public string ex;

        public HtmlPage(string url)
        {
            var web = new HtmlWeb();
            var doc = new HtmlDocument();
            try
            {
                SiteContent = web.Load(url);
                if (SiteContent.DocumentNode.Descendants("title").First().InnerText.Contains("403"))
                {
                    isForbidden = true;
                    isSuccessfully = false;
                }
            }
            catch(Exception e)
            {
                isSuccessfully = false;
                ex = e.Message;
            }            
        }

        private HtmlNode header;
        public HtmlNode Header
        {
            get
            {
                if (header is null)
                    header = SiteContent.DocumentNode.Descendants("header").FirstOrDefault();
                if (header is null)
                    header = SiteContent.DocumentNode.SelectSingleNode(@"//*[@id=""header""]");
                if (header is null)
                    header = SiteContent.DocumentNode.SelectSingleNode(@"//*[@class=""header""]");
                return header;
            }
        }

        private HtmlNode footer;
        public HtmlNode Footer
        {
            get
            {
                if (footer is null)
                    footer = SiteContent.DocumentNode.Descendants("footer").FirstOrDefault();
                if (footer is null)
                    footer = SiteContent.DocumentNode.SelectSingleNode(@"//*[@id=""footer""]");
                if (footer is null)
                    footer = SiteContent.DocumentNode.SelectSingleNode(@"//*[@class=""footer""]");
                return footer;
            }
        }

        private string title;
        public string Title
        {
            get
            {
                if (title is null)
                    title = SiteContent.DocumentNode.SelectSingleNode(@"//*[@itemprop=""name""]")?.InnerText.Trim();
                if (title is null)
                    title = SiteContent.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText.Trim();
                return title;
            }
        }
    }
}
