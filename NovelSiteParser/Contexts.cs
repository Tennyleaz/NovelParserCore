using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NovelSiteParser
{
    public class BookContext : DbContext
    {
        public DbSet<BookLink> BookLinks { get; set; }
        public DbSet<IssueLink> IssueLinks { get; set; }
        public DbSet<ChapterLink> ChapterLinks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        private readonly string _connectionString;

        public BookContext()
        {
            _connectionString = "Data Source = wenku8.db";
        }

        public BookContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public BookContext(DbContextOptions<BookContext> options)
            : base(options)
        {
        }
    }
}
