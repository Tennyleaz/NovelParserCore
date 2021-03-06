﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            var (aidString, vidString) = ParseAidVid(chapterUrl);
            Chapter chapterGot = GetChapterContent(aidString, vidString);
            if (chapterGot != null)
            {
                chapterGot.Title = tempChapter.Title;  // 標題要用舊方法 parse
                return chapterGot;
            }
            return null;
        }

        private static (string aid, string vid) ParseAidVid(string url)
        {
            // https://www.wenku8.net/novel/2/2121/109531.htm
            url = url.Replace("https://www.wenku8.net/novel/", "");
            url = url.Replace(".htm", "");
            string[] subs = url.Split('/');
            // 2/2121/109531
            if (subs.Length > 2)
            {
                string aidString = subs[1];
                string vidString = subs[2];
                return (aidString, vidString);
            }
            Console.WriteLine("Cannot parse aid/vid of url: " + url);
            return (null, null);
        }

        protected Chapter GetChapterContent(string aid, string vid)
        {
            if (string.IsNullOrEmpty(aid) || string.IsNullOrEmpty(vid))
                return null;
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

        /// <summary>
        /// 直接下載一冊txt檔案至指定路徑
        /// </summary>
        public async Task<bool> DownloadIssueAsync(IssueLink issueLink, string path)
        {
            try
            {
                if (!path.EndsWith(".txt"))
                    path += ".txt";
                string firstChapterUrl = issueLink.ChapterLinks.First().Url;
                var (aidString, vidString) = ParseAidVid(firstChapterUrl);
                // vid -1
                if (int.TryParse(vidString, out int vid))
                {
                    vid--;
                    string download = $"http://dl.wenku8.com/packtxt.php?aid={aidString}&vid={vid}&charset=big5";
                    Uri uri = new Uri(download);
                    using (WebClient client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(uri, path);
                    }
                    return true;
                }
                Console.WriteLine("Cannot parse vid: " + vidString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }
    }
}
