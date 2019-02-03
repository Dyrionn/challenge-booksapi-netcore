using Challenge.NetCore.Books.Domain;
using System.Collections.Generic;

namespace Challenge.NetCore.Books.Api.Services.Interface {
    public interface IBookService
    {
        Book AddOne(Book book);

        Book GetById(string id);

        IEnumerable<Book> GetFromExternalSource();
    }
}
