using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MangaCrawl
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://truyenqqvn.com/truyen-tranh/one-piece-128");

            var elements = driver.FindElements(By.XPath("//*[contains(@class, 'works-chapter-item')]/div/a"));
            var links = elements.ToArray().Reverse().ToList();
            var mangaName = "OnePiece";
            var chapterCount = 1;

            foreach ( var link in links )
            {
                var url = link.GetAttribute("href");
                driver.Navigate().GoToUrl(url);
                var imgElements = driver.FindElements(By.XPath("//*[contains(@class, 'page-chapter')]/img"));
                var imgSources = imgElements.ToArray().Select(i => i.GetAttribute("src"));
                var pageCount = 1;
                foreach (var img in imgSources)
                {
                    var fileName = "D:\\\\MangaCrawl\\" + mangaName + "_" + chapterCount.ToString("00") + "_" + pageCount.ToString("00") + ".jpg";
                    using (var client = new HttpClient())
                    {
                        using (var s = client.GetStreamAsync(img))
                        {
                            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                            {
                                s.Result.CopyTo(fs);
                            }
                        }
                    }
                    pageCount++;
                }
                chapterCount++;
            }

            Console.ReadLine(); 
        }
    }
}