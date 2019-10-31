using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
//using Microsoft.EntityFrameworkCore.Infrastructure;

namespace NovelSiteParser
{
    /// <summary>
    /// 書的主頁
    /// </summary>
    public class BookshelfLink
    {
        public string Title;
        public string MainPage;
    }

    /// <summary>
    /// 目錄的頁面
    /// </summary>
    public class BookLink : IEquatable<BookLink>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string IndexPage { get; set; }
        public DateTime LastUpdateTime { get; set; }

        public ICollection<IssueLink> IssueLinks { get; set; } = new List<IssueLink>();

        //public virtual ICollection<IssueLink> IssueLinks
        //{
        //    get => LazyLoader.Load(this, ref _issueLinks);
        //    set => _issueLinks = value;
        //}

        //private ILazyLoader LazyLoader { get; set; }
        //private ICollection<IssueLink> _issueLinks;

        //public BookLink() { }

        //private BookLink(ILazyLoader lazyLoader)
        //{
        //    LazyLoader = lazyLoader;
        //}

        public bool Equals(BookLink book)
        {
            if (book == null)
                return false;
            if (object.ReferenceEquals(this, book))
                return true;
            if (IndexPage != book.IndexPage)
                return false;
            if (IssueLinks == null && book.IssueLinks == null)
                return (IndexPage == book.IndexPage) && (Title == book.Title);
            if (IssueLinks?.Count != book.IssueLinks?.Count)
                return false;
            for (int i = 0; i < IssueLinks.Count; i++)
            {
                if (!IssueLinks.ElementAt(i).Equals(book.IssueLinks.ElementAt(i)))
                    return false;
            }
            return true;
        }

        public void TryAddNewIssues(BookLink newBook)
        {
            if (newBook == null || object.ReferenceEquals(this, newBook))
                return;
            if (newBook.IndexPage == IndexPage)
            {
                foreach (var newIssueLink in newBook.IssueLinks)
                {
                    IssueLink sameIssue = IssueLinks.FirstOrDefault(i => i.Title == newIssueLink.Title && i.IndexNumber == newIssueLink.IndexNumber);
                    if (sameIssue == null)
                    {   // 沒有同一冊，直接加入
                        IssueLinks.Add(newIssueLink);
                    }
                    else
                    {   // 有同一冊，但是內部章節需要更新
                        sameIssue.TryAddNewChapters(newIssueLink);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 每一冊的資料
    /// </summary>
    public class IssueLink : IEquatable<IssueLink>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public int IndexNumber { get; set; }
        public ICollection<ChapterLink> ChapterLinks { get; set; } = new List<ChapterLink>();

        public bool Equals(IssueLink issue)
        {
            if (issue == null)
                return false;
            if (object.ReferenceEquals(this, issue))
                return true;
            if (ChapterLinks == null && issue.ChapterLinks == null)
                return Title == issue.Title;
            if (ChapterLinks?.Count != issue.ChapterLinks?.Count)
                return false;
            for (int i = 0; i < ChapterLinks.Count; i++)
            {
                if (!ChapterLinks.ElementAt(i).Equals(issue.ChapterLinks.ElementAt(i)))
                    return false;
            }
            return true;
        }

        public void TryAddNewChapters(IssueLink newIssue)
        {
            if (newIssue == null || object.ReferenceEquals(this, newIssue))
                return;

            // 同一冊書才會計算
            if (newIssue.Title == Title && newIssue.IndexNumber == IndexNumber)
            {
                foreach (var newChapterLink in newIssue.ChapterLinks)
                {
                    // 沒有同一章，直接加入；有相同章節就不管
                    if (!ChapterLinks.Contains(newChapterLink))
                        ChapterLinks.Add(newChapterLink);
                }
                // sort
                //ChapterLinks = ChapterLinks.OrderBy(c => c.IndexNumber).ToList();
            }
        }

        /// <summary>
        /// 利用舊的 IssueLink 跟自己比較，算出自己冊內需要下載的章節
        /// </summary>
        public List<ChapterLink> UnDownloadedChapters(IssueLink oldIssue)
        {
            List<ChapterLink> unDownloadedChapters = new List<ChapterLink>();
            foreach (ChapterLink link in ChapterLinks)
            {
                // 如果本地的連結已經下載過，就略過
                if (oldIssue.ChapterLinks.Contains(
                    oldIssue.ChapterLinks.FirstOrDefault(x => x.Downloaded && link.Equals(x))))
                {
                    link.Downloaded = true;
                    continue;
                }
                unDownloadedChapters.Add(link);  // 剩下是此冊本地未下載的章捷
            }
            return unDownloadedChapters;
        }
    }

    /// <summary>
    /// 每一章節的資料
    /// </summary>
    public class ChapterLink : IEquatable<ChapterLink>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 此章節的內文
        /// </summary>
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Downloaded { get; set; }
        public int IndexNumber { get; set; }

        public bool Equals(ChapterLink c)
        {
            if (c == null)
                return false;
            if (Object.ReferenceEquals(this, c))
                return true;
            return Url == c.Url;
        }
    }

    public class Book
    {
        public string Title;
        public List<Issue> Issues = new List<Issue>();

        public bool SaveToTxt(string parentPath)
        {
            try
            {
                if (!Directory.Exists(parentPath))
                    Directory.CreateDirectory(parentPath);
                string newFolder = parentPath + "/" + Utilities.TrimIllegalPath(Title);
                Directory.CreateDirectory(newFolder);
                bool success = true;
                for (int i = 0; i < Issues.Count; i++)
                {
                    success = Issues[i].SaveToTxt(newFolder, i);
                    if (!success)
                        break;
                }
                return success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool SaveToSingleTxt(string filePath)
        {
            try
            {
                bool success = true;
                for (int i = 0; i < Issues.Count; i++)
                {
                    success = Issues[i].SaveToSingleTxt(filePath);
                    if (!success)
                        break;
                }
                return success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }

    public class Issue
    {
        public string Title;
        public List<Chapter> Chapters = new List<Chapter>();

        public bool SaveToTxt(string parentPath, int index)
        {
            string newFolder = parentPath + "/" + index + " - " + Utilities.TrimIllegalPath(Title);
            try
            {
                if (!Directory.Exists(parentPath))
                    Directory.CreateDirectory(parentPath);
                Directory.CreateDirectory(newFolder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            bool success = true;
            for (int i = 0; i < Chapters.Count; i++)
            {
                success = Chapters[i].SaveToTxt(newFolder, i);
                if (!success)
                    break;
            }

            return success;
        }

        public bool SaveToSingleTxt(string filePath)
        {
            bool success = true;
            for (int i = 0; i < Chapters.Count; i++)
            {
                success = Chapters[i].SaveToSingleTxt(filePath);
                if (!success)
                    break;
            }

            return success;
        }
    }

    public class Chapter
    {
        public string Title;
        public string Content;

        public Chapter(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public bool SaveToTxt(string parentPath, int index)
        {
            string fileName = parentPath + "/" + index + " - " + Utilities.TrimIllegalPath(Title) + ".txt";
            try
            {
                if (!Directory.Exists(parentPath))
                    Directory.CreateDirectory(parentPath);
                File.WriteAllText(fileName, Content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        public bool SaveToSingleTxt(string filePath)
        {
            try
            {
                File.AppendAllText(filePath, Environment.NewLine + Title + Environment.NewLine);
                File.AppendAllText(filePath, Content);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}
