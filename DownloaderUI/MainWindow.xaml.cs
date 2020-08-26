using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NovelSiteParser;
using Path = System.IO.Path;

namespace DownloaderUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Wenku8Parser2 parser;

        public MainWindow()
        {
            InitializeComponent();
            btnDownloadIssue.IsEnabled = false;
            btnGetIssues.IsEnabled = false;
        }

        private async void BtnLogin_OnClick(object sender, RoutedEventArgs e)
        {
            string username = File.ReadAllText(@"D:\Test Dir\LoginWebTest\username.txt");
            string password = File.ReadAllText(@"D:\Test Dir\LoginWebTest\password.txt");
            parser = new Wenku8Parser2(username, password, Wenku8Parser.LoginDuration.OneDay);
            parser.Init();
            if (await parser.TryLogin())
            {
                parser.SaveCookie();
                List<BookshelfLink> myBooks = await parser.GetBooksFromBookshelf();
                bookListbox.ItemsSource = myBooks;
                if (myBooks.Count > 0)
                    bookListbox.SelectedIndex = 0;
                btnLogin.IsEnabled = false;
                btnGetIssues.IsEnabled = true;
            }
        }

        private async void BtnGetIssues_OnClick(object sender, RoutedEventArgs e)
        {
            if (bookListbox.SelectedItem is BookshelfLink book)
            {
                btnGetIssues.IsEnabled = false;

                BookLink bookLinkFromWeb = await parser.FindBookIndexPageAsync(book.MainPage, CodePage.Gb2312);
                issueListbox.ItemsSource = bookLinkFromWeb.IssueLinks;
                if (bookLinkFromWeb.IssueLinks.Count > 0)
                    issueListbox.SelectedIndex = 0;
                labelIssue.Content = bookLinkFromWeb.Title;

                btnGetIssues.IsEnabled = true;
                btnDownloadIssue.IsEnabled = true;
            }
        }

        private async void BtnDownloadIssue_OnClick(object sender, RoutedEventArgs e)
        {
            if (issueListbox.SelectedItem is IssueLink issue)
            {
                btnDownloadIssue.IsEnabled = false;

                // create book directory
                string dir = Environment.CurrentDirectory + "\\" + labelIssue.Content;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // download
                string path = dir + "\\" + issue.Title + ".txt";
                await parser.DownloadIssueAsync(issue, path);

                MessageBox.Show(path, "Downloaded", MessageBoxButton.OK, MessageBoxImage.Information);
                btnDownloadIssue.IsEnabled = true;
            }
        }
    }
}
