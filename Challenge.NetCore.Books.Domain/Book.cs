using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.NetCore.Books.Domain {
    public class Book {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string isbn { get; set; }
        public string language { get; set; }
    }
}
