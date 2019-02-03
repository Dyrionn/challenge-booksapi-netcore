using Challenge.NetCore.Books.Api.Services.Interface;
using Challenge.NetCore.Books.CrossCutting;
using Challenge.NetCore.Books.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Challenge.NetCore.Books.Repository;
using Challenge.NetCore.Books.Repository.Firebase;
using Challenge.NetCore.Books.Service.Library;

namespace Challenge.NetCore.Books.Service {
    public class BookService : IBookService {

        private IBookSRepository _bookSRepository;

        public BookService(IBookSRepository bookSRepository) {
            _bookSRepository = bookSRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="book">usamos taltaltal <see cref="book"/></param>
        /// <returns></returns>
        public Book AddOne(Book book) {
            return _bookSRepository.Add(book);
        }

        public Book GetById(string id) {
            return _bookSRepository.GetById(id);
        }

        public IEnumerable<Book> GetFromExternalSource() {
            var bookList = new List<Book>();
            try {
                var content = GetStringResponseFromRequest($"https://kotlinlang.org/docs/books.html");

                if (!IsTheWebSiteStillTheSame(content))
                    return bookList; //Find a way to expose this

                //START : DEFINE PARAMETERS
                var htmltagWhereBooksListStart = "<article role=\"main\" class=\"page-content g-9\">";
                var startIndexToCut = content.IndexOf(htmltagWhereBooksListStart);
                var endIndexToCut = content.IndexOf("</article>") > -1 ? content.IndexOf("</article>") - (startIndexToCut + htmltagWhereBooksListStart.Length) : -1;
                //END : DEFINE PARAMETERS

                //START : BREAK UNTIL OBJECT
                var crudeBooksContent = content.Substring(startIndexToCut + "<article role=\"main\" class=\"page-content g-9\">".Length, endIndexToCut).Trim();

                var bookStringList = new List<string>(crudeBooksContent.RemoveLineEndings().Split("<h2", StringSplitOptions.RemoveEmptyEntries)).Select(x => "<h2" + x).ToList();

                foreach (var bookString in bookStringList) {
                    //var rawNanoParts = Regex.Split(bookString, "> *<").Where(x => x != string.Empty).ToArray();
                    var nanoParts = Regex.Split(bookString, "> *<").Where(x => x != string.Empty).Select(x => x[0].Equals('<') ? (x + ">") : "<" + x + ">").ToArray();

                    var book = new Book();
                    book.title = Regex.Replace(nanoParts[0], RegexReference.TotallyRemoveHtmlTags, "");
                    book.language = Regex.Replace(nanoParts[1], RegexReference.TotallyRemoveHtmlTags, "").ToUpperInvariant();
                    book.description = Regex.Replace(Regex.Replace(nanoParts[6], RegexReference.TotallyRemoveHtmlTags, ""), " {2,}", " ").Replace(">", "").Trim();

                    var bookLocationUrl = Regex.Match(nanoParts[2], RegexReference.GetContentBetweenEscapedDoubleQuotes).Value.Replace("\"", "");
                    book.isbn = GetIsbn(Regex.Unescape(bookLocationUrl));

                    bookList.Add(book);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
            //END : BREAK UNTIL OBJECT

            return bookList;
        }

        private string GetIsbn(string url) {
            var content = GetStringResponseFromRequest(url);

            var firstCheck = content.IndexOf("ISBN", StringComparison.InvariantCultureIgnoreCase);
            if (firstCheck > -1) {
                var maybeAnIsbn = content.Substring(firstCheck, 50);

                return Regex.Replace(maybeAnIsbn, RegexReference.GetOnlyNumbers, "");
            }

            return "Unavailable";
        }

        private bool IsTheWebSiteStillTheSame(string content) {
            var firstCheck = content.IndexOf("<a href=\"/docs/books.html\" class=\"nav-item is_active\">");
            var secondCheck = content.IndexOf("<meta name=\"twitter:site\" content=\"@kotlin\">");

            return firstCheck > -1 && secondCheck > -1;
        }

        private string GetStringResponseFromRequest(string url) {
            HttpResponseMessage httpResponseMessage = null;
            string content = string.Empty;

            using (var httpClient = new HttpClient()) {
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
                httpResponseMessage = httpClient.SendAsync(httpRequestMessage).Result;
            }

            if (httpResponseMessage != null && httpResponseMessage.IsSuccessStatusCode) {
                content = httpResponseMessage.Content.ReadAsStringAsync().Result;
            }

            return content;
        }
    }
}
