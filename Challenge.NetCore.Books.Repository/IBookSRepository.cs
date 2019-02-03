using System;
using System.Collections.Generic;
using System.Text;
using Challenge.NetCore.Books.Domain;

namespace Challenge.NetCore.Books.Repository {
    public interface IBookSRepository {
        Book Add(Book book);

        List<Book> GetAll();

        Book GetById(string id);

        Book Delete(Book book);
    }
}
