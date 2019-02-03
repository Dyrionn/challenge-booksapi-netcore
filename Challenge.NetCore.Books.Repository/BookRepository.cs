using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Challenge.NetCore.Books.Domain;
using Newtonsoft.Json;

namespace Challenge.NetCore.Books.Repository {
    public class BookRepository : IBookSRepository {

        private string _databaseUrl;
        private string _defaultDatabaseUrl = $"https://challenge-books-api.firebaseio.com/Team-Awesome/Books/";

        public BookRepository(string connectionUrl = null) {
            _databaseUrl = connectionUrl ?? _defaultDatabaseUrl;
            InitializeRepository();
        }

        private void InitializeRepository() {
            var actualAmountOfData = GetAll();
            if (actualAmountOfData == null)
                HttpRequestByMethod(HttpMethod.Put, string.Empty, "[{\"id\": \"000\", \"title\": \"placeholder\", \"description\": \"placeholder\", \"isbn\": \"0000000000000\", \"language\": \"EN\"}]");
        }

        public Book Add(Book book) {

            var actualList = GetAll();
            book.id = actualList.Count.ToString().PadLeft(3, '0');
            actualList.Add(book);

            var json = JsonConvert.SerializeObject(actualList);

            HttpRequestByMethod(HttpMethod.Put, string.Empty, json);

            return GetById(book.id);
        }

        public List<Book> GetAll() {
            var json = HttpRequestByMethod(HttpMethod.Get);

            return JsonConvert.DeserializeObject<List<Book>>(json);
        }

        public Book GetById(string id) {
            var contentResponse = HttpRequestByMethod(HttpMethod.Get, id);

            return JsonConvert.DeserializeObject<Book>(contentResponse);
        }

        public Book Delete(Book book) {
            throw new NotImplementedException();
        }

        private string HttpRequestByMethod(HttpMethod httpMethod, string resource = "", string json = null) {

            var request = WebRequest.CreateHttp($"{_databaseUrl}/{resource.TrimStart('0')}/.json");
            request.Method = httpMethod.ToString();

            if (json != null) {
                request.ContentType = "application/json";
                var buffer = Encoding.UTF8.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }

            var response = request.GetResponse();
            var content = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            return content;
        }
    }
}
