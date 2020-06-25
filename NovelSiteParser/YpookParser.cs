using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace NovelSiteParser
{
    public class YpookParser : BaseNovelSiteParser
    {
        private string currentBookMainPage = "http://www.ypook.com/28479/";

        public void Init()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            base.Init("http://www.ypook.com/");
        }

        /// <summary>
        /// 從一本書的主頁尋找目錄頁的超連結，並從目錄頁記下此書所有章節的連結
        /// </summary>
        protected override async Task<BookLink> FindBookIndexPageAsync(HtmlDocument bookDocument)
        {
            // get book status
            var tdStatusNodes = bookDocument.DocumentNode.SelectNodes("//div[contains(@class, 'novelinfo-l')]/ul/li");

            string lastUpdateTimeString = tdStatusNodes.FirstOrDefault(x => x.InnerText.StartsWith("更新："))?.InnerText;
            lastUpdateTimeString = Utilities.TrimStart(lastUpdateTimeString, "更新：");

            string authorString = tdStatusNodes.FirstOrDefault(x => x.InnerText.StartsWith("作者："))?.InnerText;
            authorString = Utilities.TrimStart(authorString, "作者：");

            var titleNode = bookDocument.DocumentNode.SelectNodes("//div[contains(@class, 'header line')]/h1");
            string titleString = titleNode.FirstOrDefault()?.InnerText;

            // parse the index page
            BookLink bookLink = GetChapterLinks(bookDocument, "http://www.ypook.com");
            bookLink.Author = Utilities.ToTraditional(authorString);
            bookLink.Title = Utilities.ToTraditional(titleString);
            if (DateTime.TryParse(lastUpdateTimeString, out DateTime lastTime))
                bookLink.LastUpdateTime = lastTime;
            return bookLink;
        }

        /// <summary>
        /// 讀取預設書架上所有書的連結
        /// </summary>
        public override async Task<List<BookshelfLink>> GetBooksFromBookshelf()
        {
            throw new NotImplementedException();
        }

        protected override Chapter GetChapterContent(HtmlDocument htmlDoc, string chapterUrl)
        {
            if (htmlDoc == null)
                return null;

            string title = string.Empty;
            HtmlNode titleDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='title']/h1");
            if (titleDiv != null)
                title = titleDiv.InnerText;

            var contents = htmlDoc.DocumentNode.SelectNodes("//div[@id='chaptercontent']");
            if (contents != null && contents.Count > 0)
            {
                string text = contents.FirstOrDefault()?.InnerHtml;
                if (!string.IsNullOrEmpty(text))
                {
                    title = Utilities.ToTraditional(title);
                    text = text.Replace("<br>", "\n");
                    text = text.Replace(@".\【零点小说网\】.手机用户输入地址：", "");
                    text = text.Replace(@"支持.\（完*本*神*站*\）.把本站分享那些需要的小伙伴！", "");
                    text = WebUtility.HtmlDecode(text);
                    //text = Utilities.ToTraditional(text);
                    Chapter chapter = new Chapter(title, text);
                    
                    Console.WriteLine("Chapter " + title);
                    return chapter;
                }
            }
            return null;
        }

        protected override BookLink GetChapterLinks(HtmlDocument htmlDoc, string baseUrl)
        {
            BookLink bookLink = new BookLink();
            // 此書只有一冊
            IssueLink issueLink = new IssueLink();
            issueLink.Title = "卷一";
            bookLink.IssueLinks.Add(issueLink);

            int chapterIndex = 0;
            var chapterNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='card mt20 fulldir']/div/ul/li");
            foreach (var node in chapterNodes)
            {
                var link = node.Descendants("a").FirstOrDefault();
                if (link != null)
                {
                    ChapterLink chapterLink = new ChapterLink();
                    chapterLink.Title = Utilities.ToTraditional(link.InnerText);
                    chapterLink.Url = baseUrl + link.Attributes["href"]?.Value;
                    chapterLink.IndexNumber = chapterIndex;
                    chapterIndex++;  // 要自己維護 list 的順序
                    issueLink.ChapterLinks.Add(chapterLink);
                }
            }
            return bookLink;
        }

        public override bool SaveCookie()
        {
            throw new NotImplementedException();
        }

        protected override CookieContainer LoadCookie()
        {
            throw new NotImplementedException();
        }
    }
}
