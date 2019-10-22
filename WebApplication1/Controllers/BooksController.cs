

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using WebApiApp.Models;
using WebApiApp.Services;


namespace WebApiApp.Controllers
{
 
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public BooksController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet()]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var booksForAuthorFromRepo = _libraryRepository.GetBooksForAuthor(authorId);

            var booksForAuthor = from c in booksForAuthorFromRepo
                                 select new BookDto()
                                 {
                                     Id = c.Id,
                                     Title = c.Title,
                                     Description = c.Description,
                                     AuthorId = c.AuthorId                                   
                                  };

            //var booksForAuthor = Mapper.Map<IEnumerable<BookDto>>(booksForAuthorFromRepo);

            return Ok(booksForAuthor);
        }

        [HttpGet("{id}")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = _libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                return NotFound();
            }

            var bookForAuthor = new BookDto()
            {
                Id = bookForAuthorFromRepo.Id,
                Title = bookForAuthorFromRepo.Title,
                Description = bookForAuthorFromRepo.Description,
                AuthorId = bookForAuthorFromRepo.AuthorId
            };

            //var bookForAuthor = Mapper.Map<BookDto>(bookForAuthorFromRepo);
            return Ok(bookForAuthor);
        }
    }
}
