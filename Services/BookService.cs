using AutoMapper;
using BookCatalog.MicroService.DTOs;
using BookCatalog.MicroService.Entities;
using BookCatalog.MicroService.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using BookCatalog.MicroService.Messaging.Send.Sender;

namespace BookCatalog.MicroService.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _bookrepository;
        private List<Book> _bookDetails;
        private readonly IMapper _mapper;
        private IBookSender _bookSender;
        public BookService(IBookRepository bookrepository, IMapper mapper, IBookSender bookSender)
        {
            _bookDetails = new List<Book>();
            _bookrepository = bookrepository;
            _mapper = mapper;
            _bookSender = bookSender;
        }

        public IEnumerable<BookDTO> Book => _bookrepository.GetBook().ProjectTo<BookDTO>(_mapper.ConfigurationProvider);

        public IEnumerable<BookDTO> AddBook(BookDTO bookdto)
        {

            var result = _bookrepository.AddBook(_mapper.Map<Book>(bookdto)).ProjectTo<BookDTO>(_mapper.ConfigurationProvider);
            if (result.Any())
            {
                _bookSender.SendMessagetoQueue("Book Added");
            }
            return result;
        }

        public IEnumerable<BookDTO> UpdateBook(BookDTO bookdto)
        {
            var result = _bookrepository.UpdateBook(_mapper.Map<Book>(bookdto)).ProjectTo<BookDTO>(_mapper.ConfigurationProvider);
            if (result.Any())
            {
                _bookSender.SendMessagetoQueue("Book Updated");
            }
            return result;
        }

        public string DeleteBook(string id)
        {
            var result = _bookrepository.DeleteBook(id);
            if (!string.IsNullOrEmpty(result))
            {
                _bookSender.SendMessagetoQueue("Book Deleted");
            }
            return result;
        }


        public IEnumerable<BookDTO> GetBooks(string title, string author, string isbn)
        {
            return _bookrepository.GetBooks(title, author, isbn).ProjectTo<BookDTO>(_mapper.ConfigurationProvider);
        }

    }

}
