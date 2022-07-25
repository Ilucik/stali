using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace контакты
{
    public class NodeParser
    {
        public static string GetMail(HtmlPage CompanyPage)
        {
            string mail = null;
            var itemprop = CompanyPage.SiteContent.DocumentNode.SelectSingleNode(@"//*[@itemprop=""email""]");
            if (mail is null && itemprop is not null)
                mail = itemprop.GetAttributeValue("content", null)?.Trim();
            if (mail is null && itemprop is not null)
                mail = itemprop.InnerText.Trim();
            if (mail is null && CompanyPage.Header is not null)
                mail = ParseMail(CompanyPage.Header);
            if (mail is null && CompanyPage.Footer is not null)
                mail = ParseMail(CompanyPage.Footer);
            if (mail is null)
                mail = "";
            return mail;
        }

        private static string ParseMail(HtmlNode node)
        {
            var nodes = node.SelectNodes("//*[contains(., '@')]");
            if (nodes == null)
                return "";
            var str = nodes.LastOrDefault()?.InnerText;
            var res = Regex.Match(str, @"[A-z0-9_.+-]+@[A-z.-]+").Value.Trim();
            if (res.Length == 0)
                res = node.SelectSingleNode(@"//a[starts-with(@href, ""mailto"")]")
                    .GetAttributeValue("href",null)
                    .Split(':')[1];
            return res;
        } 

        public static string GetPhone (HtmlPage CompanyPage)
        {
            string phone = null;
            var itemprop = CompanyPage.SiteContent.DocumentNode.SelectSingleNode(@"//*[@itemprop=""telephone""]");
            if (phone is null && itemprop is not null)
                phone = itemprop.GetAttributeValue("content", null)?.Trim();
            if (phone is null && itemprop is not null)
                phone = itemprop.InnerText.Trim();            
            if (phone is null && CompanyPage.Header is not null)
                phone = ParcePhone(CompanyPage.Header);
            if (phone is null && CompanyPage.Footer is not null)
                phone = ParcePhone(CompanyPage.Footer);
            if (phone is null)
                phone = "";
            return phone;
        }
        private static string ParcePhone(HtmlNode node)
        {
            var nodes = node.SelectNodes("//*[contains(., '+7')]");
            if (nodes == null)
                nodes = node.SelectNodes("//*[contains(., '8')]");
            if (nodes == null)
                return "";
            var str = nodes.FirstOrDefault().InnerText;
            return Regex.Match(str, @"(\s*)?(\+)?([- _():=+]?\d[- _():]?){10,14}(\s*)?").Value.Trim();
        }
    }
}
