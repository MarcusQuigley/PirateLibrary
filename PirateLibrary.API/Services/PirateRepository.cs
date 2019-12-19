using PirateLibrary.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PirateLibrary.API.Services
{
    public class PirateRepository : IPirateRepository
    {
        readonly PirateLibraryContext context;

        public PirateRepository(PirateLibraryContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool Save()
        {
            return (context.SaveChanges() >= 0);
        }

        public int Count()
        {
            return context.Authors.Count();
        }

        public Author GetAuthor(Guid authorid)
        {
            if (authorid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorid));
            }
            return context.Authors.FirstOrDefault(a => a.Id == authorid);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return context.Authors;
        }

        public IEnumerable<Author> GetAuthors(AuthorsResourceParameters parms)
        {
            if (string.IsNullOrWhiteSpace(parms.MainCategory)
                && string.IsNullOrWhiteSpace(parms.SearchQuery))
            {
                return GetAuthors();
            }
            var authors = context.Authors as IQueryable<Author>;

            if (!string.IsNullOrWhiteSpace(parms.MainCategory))
            {
                var mainCategory = parms.MainCategory.Trim();
                authors = authors.Where(a => a.MainCategory == mainCategory);
            }
            if (!string.IsNullOrWhiteSpace(parms.SearchQuery))
            {
                var searchQuery = parms.SearchQuery.Trim();
                authors = authors.Where(a => a.MainCategory.Contains(searchQuery)
                || a.FirstName.Contains(searchQuery)
                || a.LastName.Contains(searchQuery));
            }
            return authors;
        }

        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }
            return context.Authors.Any(a => a.Id == authorId);
        }

        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            author.Id = Guid.NewGuid();
            context.Authors.Add(author);
        }

        public void UpdateAuthor(Author author)
        {
            //nowt to do
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }
            context.Authors.Remove(author);
        }
    }
}
