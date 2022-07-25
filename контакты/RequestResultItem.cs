using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace контакты
{
    public class RequestResultItem
    {
        public HtmlNode Markup { get; }
        private string description;
        public bool isForbidden;
        public bool isSuccessfully;

        public RequestResultItem(HtmlNode markup)
        {
            Markup = markup;
            CompanyPage = new HtmlPage(Link);
            isSuccessfully = CompanyPage.isSuccessfully;
            isForbidden = CompanyPage.isForbidden;
        }

        public RequestResultItem(string link)
        {
            this.link = link;
            CompanyPage = new HtmlPage(Link);
            description = "";
            isSuccessfully = CompanyPage.isSuccessfully;
            isForbidden = CompanyPage.isForbidden;
        }


        private HtmlPage CompanyPage { get; set; }

        public string Description
        {
            get 
            { 
                if (description is null)
                    description = Markup.SelectSingleNode(@".//*[@aria-level=""3""]")?.InnerText;
                return description;
            }
        }

        string phone;
        public string Phone
        {
            get
            {
                if (phone is null)
                    phone = NodeParser.GetPhone(CompanyPage);
                return phone;
            }
        }

        string mail;
        public string Mail
        {
            get
            {
                if (mail is null)
                    mail = NodeParser.GetMail(CompanyPage);
                return mail;
            }
        }

        private string link;

        public string Link
        {
            get
            {
                if (link is null)
                    link = Markup.Descendants("a").First().GetAttributeValue("href", "");
                return new Uri(link).GetLeftPart(UriPartial.Authority);
            }
        }

        public  string Parse()
        {
            if (isForbidden)
                return String.Join("\r\n", Description, Link, "403 Forbidden");
            if (!isSuccessfully)
                return String.Join("\r\n", Description, Link, CompanyPage.ex);
            return String.Join("\r\n",CompanyPage.Title, Description, Link, Phone, Mail);
        }
    }
}
