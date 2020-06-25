using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NovelSiteParser
{
    public class Wenku8Parser2 : Wenku8Parser
    {
        // aid 是書的代碼，vid是章節代碼
        // dl.wenku8.com/packtxt.php?aid=2121&vid=109531&charset=big5

        public Wenku8Parser2(string username, string password, LoginDuration duration = LoginDuration.None)
            : base(username, password, duration)
        {

        }

        protected override Chapter GetChapterContent(HtmlDocument htmlDoc, string chapterUrl)
        {
            if (htmlDoc == null)
                return null;

            Chapter tempChapter = base.GetChapterContent(htmlDoc, chapterUrl);

            string url = chapterUrl;
            // https://www.wenku8.net/novel/2/2121/109531.htm
            url = url.Replace("https://www.wenku8.net/novel/", "");
            url = url.Replace(".htm", "");
            string[] subs = url.Split('/');
            // 2/2121/109531
            if (subs.Length > 2)
            {
                string aidString = subs[1];
                string vidString = subs[2];
                Chapter chapterGot = GetChapterContent(aidString, vidString);
                if (chapterGot != null)
                {
                    chapterGot.Title = tempChapter.Title;  // 標題要用舊方法 parse
                    return chapterGot;
                }
            }
            return null;
        }

        protected Chapter GetChapterContent(string aid, string vid)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string uri = $"http://dl.wenku8.com/packtxt.php?aid={aid}&vid={vid}&charset=big5";
                    string path = AppDomain.CurrentDomain.BaseDirectory + aid + "-" + vid + ".txt";
                    client.DownloadFile(uri, path);

                    string chapterText = File.ReadAllText(path);
                    Chapter chapter = new Chapter(null, chapterText);
                    File.Delete(path);
                    return chapter;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }
    }
}
