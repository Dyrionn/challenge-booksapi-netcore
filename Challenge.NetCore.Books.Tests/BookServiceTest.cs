using AutoFixture;
using Challenge.NetCore.Books.Domain;
using Challenge.NetCore.Books.Repository;
using Challenge.NetCore.Books.Service;
using Moq;
using System;
using Xunit;

namespace Challenge.NetCore.Books.Tests
{
    public class BookServiceTest
    {
        private BookService _bookService;
        private Mock<IBookSRepository> _repositoryMock;
        private Fixture _fixture;
        public BookServiceTest()
        {
            _repositoryMock = new Mock<IBookSRepository>();
            _fixture = new Fixture();
            Setup();
        }

        [Fact]
        private void AddBook_GivenNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => _bookService.AddOne(null));
        }

        [Fact]
        private void AddBook_GivenValidData_Proceed()
        {
            var book = _fixture.Create<Book>();
            _bookService.AddOne(book);

            _repositoryMock.Verify(s => s.Add(It.IsAny<Book>()), Times.Once());
        }

        [Fact]
        private void GetById_GivenNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => _bookService.GetById(null));
        }

        #region private Methods
        private void Setup()
        {

            _repositoryMock.Setup(s => s.Add(null)).Throws<Exception>();
            _repositoryMock.Setup(s => s.GetById(null)).Throws<Exception>();

            _bookService = new BookService(_repositoryMock.Object);
        }
        #endregion
    }
}
