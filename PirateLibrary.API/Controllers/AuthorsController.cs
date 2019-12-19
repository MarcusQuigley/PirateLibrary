﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpHead]
        public ActionResult GetAuthors([FromQuery] AuthorsResourceParameters parms)
           // [FromQuery] string mainCategory, string searchQuery)
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
        [Route("count")]
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

    }
}
