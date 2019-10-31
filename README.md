# NovelParserCore project
An improved verson of my previous novel parser. Using dotnet core.

## BaseNovelSiteParser class
This abstract class provides some abstract method to parse given online model site, and methods to scrap url from a webpage.

## Wenku8Parser class
The actual parser to scrap Wenku8 site.

It will login to Wenku8 via given username/password, then get books form user's bookshelf.

## YpookParser class
The actual parser to scrap Ypook site. Ypook does't require login, not using cookie.

# KindleMail project
To send books to my kindle via email.

(Under construction)
