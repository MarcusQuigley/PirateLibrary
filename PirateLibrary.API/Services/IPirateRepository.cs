using PirateLibrary.API.Entities;
using System;
using System.Collections.Generic;

namespace PirateLibrary.API.Services
{
    public interface IPirateRepository
    {
        bool AuthorExists(Guid authorId);
        Author GetAuthor(Guid authorid);
        IEnumerable<Author> GetAuthors();

        IEnumerable<Author> GetAuthors(AuthorsResourceParameters parms);
        void AddAuthor(Author author);

        void UpdateAuthor(Author author);
        bool Save();

        int Count();
    }
}