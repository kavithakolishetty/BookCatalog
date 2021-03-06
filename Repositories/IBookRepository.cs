using BookCatalog.MicroService.Entities;
using System.Linq;

namespace BookCatalog.MicroService.Repositories
{
    public interface IBookRepository
    {
        public IQueryable<Book> GetBook();
        public IQueryable<Book> AddBook(Book book);
        public IQueryable<Book> UpdateBook(Book book);
        public string DeleteBook(string id);
        public IQueryable<Book> GetBooks(string title, string author, string isbn);
    }
}
