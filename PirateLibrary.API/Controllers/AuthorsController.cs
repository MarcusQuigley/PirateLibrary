using Microsoft.AspNetCore.Mvc;
using PirateLibrary.API.Entities;
using PirateLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PirateLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        readonly IPirateRepository service;
        public AuthorsController(IPirateRepository service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public ActionResult GetAuthors()
        {
            return Ok(service.GetAuthors());
        }

        [HttpGet("{authorId}",Name ="GetAuthorPath")]
        public ActionResult<Author> GetAuthor(Guid authorId)
        {
            if (!service.AuthorExists(authorId))
            {
                return BadRequest();
            }
            return Ok(service.GetAuthor(authorId));
        }

        [HttpPost]
        public ActionResult AddAuthor(Author author)
        {
            if (author == null)
            {
                return BadRequest();
            }
            service.AddAuthor(author);
            service.Save();
            return Ok();

        }

    }
}
