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

       

       
    }
}
