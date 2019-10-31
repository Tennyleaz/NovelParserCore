using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NovelSiteParser;

namespace NovelParserCore
{
    class Program
    {
        private const string sqliteString = "Data Source = wenku8.db";

        static async Task Main(string[] args)
        {
            await Test();
            using (BookContext bookContext = new BookContext(sqliteString))
            {
                bookContext.Database.EnsureCreated();
                bookContext.Database.OpenConnection();

                // test with db
                bookContext.Set<ChapterLink>().Load();
                bookContext.Set<IssueLink>().Load();
                bookContext.Set<BookLink>().Load();
                var testBook0 = bookContext.BookLinks.FirstOrDefault();
                Console.WriteLine(testBook0?.IndexPage);

                string username = File.ReadAllText(@"D:\Test Dir\LoginWebTest\username.txt");
                string password = File.ReadAllText(@"D:\Test Dir\LoginWebTest\password.txt");
                Wenku8Parser parser = new Wenku8Parser(username, password, Wenku8Parser.LoginDuration.OneDay);
                parser.Init();
                if (await parser.TryLogin())
                {
                    parser.SaveCookie();
                    List<BookshelfLink> myBooks = await parser.GetBooksFromBookshelf();
                    foreach (var bookshelfLink in myBooks)
                    {
                        BookLink bookLinkFromWeb = await parser.FindBookIndexPageAsync(bookshelfLink.MainPage, CodePage.Gb2312);
                        // update book to database (update or add new)
                        await TryAddNewBook(bookContext, bookLinkFromWeb);
                        bookContext.SaveChanges();

                        // download book
                        parser.OnlyReturnNew = true;
                        BookLink bookLinkFromDB = await bookContext.BookLinks.FirstOrDefaultAsync(b => b.IndexPage == bookLinkFromWeb.IndexPage);
                        Book book = await parser.GetBookAsync(bookLinkFromDB, CodePage.Gb2312);
                        bookContext.SaveChanges();

                        book.SaveToTxt(Environment.CurrentDirectory);

                        // test with db
                        //bookContext = new BookContext();
                        //bookContext.Database.EnsureCreated();
                        //bookContext.Database.OpenConnection();
                        var testBook = await bookContext.BookLinks.FirstOrDefaultAsync();
                        Console.WriteLine(testBook.IndexPage);
                        //bookContext.Database.CloseConnection();
                        //bookContext.Dispose();
                        return;

                        // download book
                        //Book book = await parser.GetBookAsync(bookLink);

                        // test with db after get book content
                        bookContext.SaveChanges();
                        var testBook2 = await bookContext.BookLinks.FirstOrDefaultAsync();
                        Console.WriteLine(testBook2.IndexPage);
                        bookContext.Database.CloseConnection();

                        // save txt to file
                        //book.SaveToTxt(AppDomain.CurrentDomain.BaseDirectory);
                    }
                }
                else
                {
                    Console.WriteLine("Login failed!");
                }
            }

            Console.ReadLine();
        }

        private static async Task TryAddNewBook(BookContext bookContext, BookLink newBookLink)
        {
            // DbSet 不能使用 IEquatable.Equals，只能使用能轉換成 SQL 的 Linq 語法
            BookLink existingBook = await bookContext.BookLinks.FirstOrDefaultAsync(b => b.IndexPage == newBookLink.IndexPage);
            if (existingBook != null)
            {
                // 檢查書內每一冊，都要加入現有 DbSet
                existingBook.TryAddNewIssues(newBookLink);
                return;
            }
            // 此書不存在，直接加入 DbSet
            bookContext.BookLinks.Add(newBookLink);
        }

        private static async Task Test()
        {
            string f = File.ReadAllText(@"D:\Test Dir\NovelParserCore\诡境主宰章节.html");
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(f);
            //FindBookIndexPageAsync(htmlDoc);
            //GetChapterLinks(htmlDoc, "https://www.wenku8.net/novel/1/1435/index.htm");
            YpookParser ypookParser = new YpookParser();
            ypookParser.Init();
            var bookLink = await ypookParser.FindBookIndexPageAsync("http://www.ypook.com/28479/");

            // download
            List<ChapterLink> cl = bookLink.IssueLinks.First().ChapterLinks.ToList();
            cl.RemoveRange(0, 730);
            bookLink.IssueLinks.First().ChapterLinks = cl;
            Book book = await ypookParser.GetBookAsync(bookLink, CodePage.Utf8);
            book.SaveToSingleTxt(@"D:\Test Dir\NovelParserCore\Book.txt");
        }
    }
}
