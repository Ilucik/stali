using HtmlAgilityPack;
using System.Diagnostics;
using контакты;

var sw = new Stopwatch();
sw.Start();

var request = "шестигранник калиброванный 38 XC".Replace(' ', '+');
var url = "http://www.google.com/search?q=" + request;
const bool SKIPFORBIDDEN = true;
const int PAGECOUNT = 5;

var count = 0;
var phoneCount = 0;
var fCount = 0;

var web = new HtmlWeb();
var doc = new HtmlDocument();


for (var i = 0; i < PAGECOUNT; i++)
{
    doc = web.Load(url + $"&start={i}0");
    var links = doc.DocumentNode.SelectNodes(@"//*[@class=""g""]");
    if (links is null)
        break;
    //var links = new List<string>()
    //{
    //    "http://www.metallotorg.ru/"
    //};
    foreach (var link in links)
    {
        var req = new RequestResultItem(link);
        count++;
        if (req.isSuccessfully || !SKIPFORBIDDEN)
        {
            Console.WriteLine(req.Parse());
            Console.WriteLine();

            if (req.isSuccessfully && req.Phone.Length > 0)
                phoneCount++;
        }

        if (req.isForbidden)
            fCount++;        
    }
}

sw.Stop();

Console.WriteLine("Количество сайтов: " + count);
Console.WriteLine("Количество телефонов: " + phoneCount);
Console.WriteLine("403 Forbidden: " + fCount);
Console.WriteLine("Время запроса: " + sw.Elapsed.TotalSeconds);

