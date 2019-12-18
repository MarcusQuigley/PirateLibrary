using Microsoft.AspNetCore.Mvc;
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
    }
}
