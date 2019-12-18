using PirateLibrary.API.Entities;
using System;
using System.Collections.Generic;

namespace PirateLibrary.API.Services
{
    public interface IPirateRepository
    {
        Author GetAuthor(Guid authorid);
        IEnumerable<Author> GetAuthors();
    }
}