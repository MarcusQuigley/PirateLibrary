using Microsoft.AspNetCore.JsonPatch;
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
        [HttpHead] //doesnt return body. Just used to get status code
        public ActionResult GetAuthors([FromQuery] AuthorsResourceParameters parms)
           // [FromQuery] string mainCategory, string searchQuery)
           //[FromQuery] states that were passing in a querystring
        {
            return Ok(service.GetAuthors(parms));
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        public ActionResult<Author> GetAuthor(Guid authorId)
        {
            if (!service.AuthorExists(authorId))
            {
                return BadRequest();
            }
            return Ok(service.GetAuthor(authorId));
        }
        [HttpGet]
        [Route("count")] //add /count to the existing route
        public ActionResult Count()
        {
            return Ok(service.Count());
        }

        [HttpPost]
        public ActionResult<Author> AddAuthor(Author author)
        {
            if (author == null)
            {
                return BadRequest();
            }
            service.AddAuthor(author);
            service.Save();
            return CreatedAtRoute("GetAuthor",
                new { authorId = author.Id }, author);

        }

        [HttpPatch("{authorId}")]
        public ActionResult PatchAuthor(Guid authorId, JsonPatchDocument<Author> patchDocument)
        {
            if (!service.AuthorExists(authorId))
            {
                return BadRequest();
            }

            var author = service.GetAuthor(authorId);
            if (authorId == null)
            {
                return NotFound();
            }
            patchDocument.ApplyTo(author);
            service.UpdateAuthor(author);
            service.Save();
            
            return NoContent();

        }
    }
}
