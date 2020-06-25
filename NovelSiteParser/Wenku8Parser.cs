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
    public class Wenku8Parser : BaseNovelSiteParser
    {
        private readonly string username, password;
        private readonly LoginDuration cookieTimeout;
        private const string INDEX_PAGE = "https://www.wenku8.net/index.php";
        private const string LOGIN_PAGE = "https://www.wenku8.net/login.php";
        private const string LOGIN_SUBMIT = "https://www.wenku8.net/login.php?do=submit";
        private const string BOOKSHELF = "https://www.wenku8.net/modules/article/bookcase.php";
        private const string COOKIE_PATH = "wenku8.dat";

        public Wenku8Parser(string username, string password, LoginDuration duration = LoginDuration.None)
        {
            this.username = username;
            this.password = password;
            this.cookieTimeout = duration;
        }

        public void Init()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            base.Init(LOGIN_PAGE, LoadCookie());
        }

        #region Login methods

        public async Task<bool> IsUserLogin()
        {
            using (var testHandler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = false })
            {
                using (HttpClient testClient = new HttpClient(testHandler) { BaseAddress = new Uri(INDEX_PAGE) })
                {
                    try
                    {
                        var homePageResult = await testClient.GetAsync(INDEX_PAGE);
                        if (homePageResult.IsSuccessStatusCode)
                            return true;
                        else if (homePageResult.StatusCode == HttpStatusCode.Redirect)
                        {
                            Console.WriteLine("302 redirected.");
                        }
                        else
                        {
                            Console.WriteLine("Unhandled status code: " + homePageResult.StatusCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            return false;
        }

        public async Task<bool> IsLoginPageAvailable()
        {
            try
            {
                var homePageResult = await httpClient.GetAsync("/");
                homePageResult.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public async Task<bool> Login()
        {
            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                    new KeyValuePair<string, string>("usecookie", cookieTimeout.ToString()),
                    new KeyValuePair<string, string>("action", "login")
                });
                var loginResult = await httpClient.PostAsync(LOGIN_SUBMIT, content);
                loginResult.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        /// <summary>
        /// 利用上次的 cookie 看能不能直接讀取。不行再用帳號密碼登入。
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TryLogin()
        {
            if (await IsUserLogin())
            {
                Console.WriteLine("Login using saved session.");
                return true;
            }
            return await Login();
        }

        #endregion
        
        /// <summary>
        /// 從一本書的主頁尋找目錄頁的超連結，並從目錄頁記下此書所有章節的連結
        /// </summary>
        protected override async Task<BookLink> FindBookIndexPageAsync(HtmlDocument bookDocument)
        {
            // get book status
            var tdStatusNodes = bookDocument.DocumentNode.SelectNodes("//td[@width='20%']");
            string lastUpdateTimeString = tdStatusNodes.FirstOrDefault(x => x.InnerText.StartsWith("最后更新："))?.InnerText;
            lastUpdateTimeString = Utilities.TrimStart(lastUpdateTimeString, "最后更新：");
            // get index page link
            string indexHref = FindTargetHref(bookDocument, "小说目录");
            // get index page body
            string htmlString2 = await GetPageBodyAsync(indexHref, CodePage.Gb2312);
            HtmlDocument bookPageDocument = new HtmlDocument();
            bookPageDocument.LoadHtml(htmlString2);
            // parse the index page
            BookLink bookLink = GetChapterLinks(bookPageDocument, indexHref);
            if (DateTime.TryParse(lastUpdateTimeString, out DateTime lastTime))
                bookLink.LastUpdateTime = lastTime;
            return bookLink;
        }

        /// <summary>
        /// 讀取預設書架上所有書的連結
        /// </summary>
        public override async Task<List<BookshelfLink>> GetBooksFromBookshelf()
        {
            List<BookshelfLink> bookshelfLinks = new List<BookshelfLink>();

            // get bookcase page body
            string htmlString = await GetPageBodyAsync(BOOKSHELF, CodePage.Gb2312);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);

            // select a node from bookcase
            var checkFormNode = htmlDoc.DocumentNode.SelectSingleNode("//form[@name='checkform']");
            var tableNode = checkFormNode.SelectSingleNode("//table");
            if (tableNode != null)
            {
                foreach (HtmlNode row in tableNode.SelectNodes("tr"))
                {
                    var columns = row.SelectNodes("th|td");
                    if (columns != null && columns.Count >= 7)
                    {
                        var secondColumn = columns[1];
                        var link = secondColumn.Descendants("a").FirstOrDefault();
                        if (link != null)
                        {
                            BookshelfLink bookshelfLink = new BookshelfLink
                            {
                                Title = Utilities.ToTraditional(link.InnerText),
                                MainPage = link.Attributes["href"]?.Value
                            };
                            bookshelfLinks.Add(bookshelfLink);
                        }
                    }
                }
            }

            return bookshelfLinks;
        }

        protected override Chapter GetChapterContent(HtmlDocument htmlDoc, string chapterUrl)
        {
            if (htmlDoc == null)
                return null;

            string title = string.Empty;
            HtmlNode titleDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='title']");
            if (titleDiv != null)
                title = titleDiv.InnerText;

            // remove <ul id="contentdp">
            var contentDps = htmlDoc.DocumentNode.SelectNodes("//ul[@id='contentdp']");
            foreach (var dp in contentDps)
            {
                dp.Remove();
            }

            var contents = htmlDoc.DocumentNode.SelectNodes("//div[@id='content']");
            if (contents != null && contents.Count > 0)
            {
                string text = contents.FirstOrDefault()?.InnerText;
                if (!string.IsNullOrEmpty(text))
                {
                    text = WebUtility.HtmlDecode(text);
                    title = Utilities.ToTraditional(title);
                    text = Utilities.ToTraditional(text);
                    Chapter chapter = new Chapter(title, text);
                    return chapter;
                }
            }
            return null;
        }

        protected override BookLink GetChapterLinks(HtmlDocument htmlDoc, string baseUrl)
        {
            var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='title']");
            var authorNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='info']");

            BookLink bookLink = new BookLink
            {
                IndexPage = baseUrl,
                Title = Utilities.ToTraditional(titleNode?.InnerText),
                Author = authorNode?.InnerText
            };

            baseUrl = Utilities.TrimEnd(baseUrl.ToLower(), "index.htm");

            List<HtmlNode> tableRows = new List<HtmlNode>();
            var tables = htmlDoc.DocumentNode.SelectNodes("//table[@class='css']");
            foreach (var tableNode in tables)
            {
                tableRows.AddRange(tableNode.Descendants("tr"));
            }

            int issueIndex = 0;
            int chapterIndex = 0;
            List<IssueLink> issueLinks = new List<IssueLink>();
            foreach (var node in tableRows)
            {
                var titleRow = node.SelectSingleNode(".//td[@class='vcss']");
                if (titleRow != null)
                {
                    IssueLink b = new IssueLink
                    {
                        Title = Utilities.ToTraditional(titleRow.InnerText),
                        IndexNumber = issueIndex,
                    };
                    issueLinks.Add(b);
                    issueIndex++;
                    chapterIndex = 0;  // 讀到新一冊就重置章節記數
                    continue;
                }

                // 一個章節的 tr 裡面會有一個以上的 ccss 文章超連結
                var chapterRows = node.SelectNodes(".//td[@class='ccss']");
                foreach (var chapterRow in chapterRows)
                {
                    var link = chapterRow.Descendants("a").FirstOrDefault();
                    if (link != null)
                    {
                        ChapterLink chapterLink = new ChapterLink();
                        chapterLink.Title = Utilities.ToTraditional(link.InnerText);
                        chapterLink.Url = baseUrl + link.Attributes["href"]?.Value;
                        chapterLink.IndexNumber = chapterIndex;
                        chapterIndex++;  // 要自己維護 list 的順序
                        issueLinks.Last()?.ChapterLinks.Add(chapterLink);
                    }
                }
            }

            bookLink.IssueLinks = issueLinks;
            return bookLink;
        }

        public override bool SaveCookie()
        {
            return WriteCookiesToDisk(COOKIE_PATH, base.cookieContainer);
        }

        protected override CookieContainer LoadCookie()
        {
            return ReadCookiesFromDisk(COOKIE_PATH);
        }

        public enum LoginDuration
        {
            None = 0,
            OneDay = 86400,
            OneMonth = 2592000,
            OneYear = 315360000
        }
    }
}
