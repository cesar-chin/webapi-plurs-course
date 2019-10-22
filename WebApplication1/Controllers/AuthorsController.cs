

using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using WebApiApp.Helpers;
using WebApiApp.Models;
using WebApiApp.Services;


namespace WebApiApp.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private ILibraryRepository _libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = _libraryRepository.GetAuthors();
            
            var authors = from c in authorsFromRepo
                       select new AuthorDto ()   { 
                           Id  = c.Id,
                           Name = c.FirstName + c.LastName,
                           Genre = c.Genre 
            };

            //var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthor(Guid id)
        {
            var authorFromRepo = _libraryRepository.GetAuthor(id);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            var author = new AuthorDto()
                         {
                             Id = authorFromRepo.Id,
                             Name = authorFromRepo.FirstName + authorFromRepo.LastName,
                             Genre = authorFromRepo.Genre
                         };

            //todo: https://medium.com/@cdelgado1978/implementando-automapper-sin-perderte-en-el-camino-213e11af72c1
            //var author = Mapper.Map<AuthorDto>(authorFromRepo);
            return Ok(author);
        }
    }
}
