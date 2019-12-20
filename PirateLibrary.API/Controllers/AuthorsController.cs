using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PirateLibrary.API.Entities;
using PirateLibrary.API.Models;
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
        readonly IMapper mapper;
        public AuthorsController(IPirateRepository service, IMapper mapper)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [HttpHead] //doesnt return body. Just used to get status code
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsResourceParameters parms)
           // [FromQuery] string mainCategory, string searchQuery)
           //[FromQuery] states that were passing in a querystring
        {
            var authors = service.GetAuthors(parms);
            var authorsDto = mapper.Map<IEnumerable<AuthorDto>>(authors);
            return Ok(authorsDto);
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                return BadRequest();
            }
            if (!service.AuthorExists(authorId))
            {
                return NotFound();
            }

            var author = service.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AuthorDto>(author));
        }
        [HttpGet]
        [Route("count")] //add /count to the existing route
        public IActionResult Count()
        {
            return Ok(service.Count());
        }

        [HttpPost]
        public ActionResult<AuthorDto> AddAuthor(AuthorForCreationDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }
            var authorEntity = mapper.Map<Author>(author);
            
            service.AddAuthor(authorEntity);
            service.Save();
            var authorDto = mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor",
                new { authorId = authorEntity.Id }, authorDto);

        }

        [HttpPatch("{authorId}")]
        public IActionResult PatchAuthor(Guid authorId, JsonPatchDocument<AuthorForUpdateDto> patchDocument)
        {
            if (!service.AuthorExists(authorId))
            {
                return NotFound();
            }

            var author = service.GetAuthor(authorId);
            if (authorId == null)
            {
                return NotFound();
            }
            var authorToPatch = mapper.Map<AuthorForUpdateDto>(author);
            patchDocument.ApplyTo(authorToPatch, ModelState);
            if (!TryValidateModel(authorToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(authorToPatch, author);
            service.UpdateAuthor(author);
            service.Save();
 
            return NoContent();
         }

        [HttpPut("{authorId}")]
        public IActionResult UpdateAuthor(Guid authorId, AuthorForUpdateDto author)
        {
            if (!service.AuthorExists(authorId))
            {
                return BadRequest();
            }

            var authorEntity = mapper.Map<Entities.Author>(author);
            service.UpdateAuthor(authorEntity);
            service.Save();
            return NoContent();
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            if (!service.AuthorExists(authorId))
            {
                return NotFound();
            }
            var author = service.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            service.DeleteAuthor(author);
            service.Save();
            
            return NoContent();
        }

        [HttpGet("{authorId}/testOnly")]
        public ActionResult GetUnMappedAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                return BadRequest();
            }
            if (!service.AuthorExists(authorId))
            {
                return NotFound();
            }

            var author = service.GetAuthor(authorId);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            base.Response.Headers.Add("Allow", "GET, POST, OPTIONS, PATCH, DELETE");
            return Ok();
        }


        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
