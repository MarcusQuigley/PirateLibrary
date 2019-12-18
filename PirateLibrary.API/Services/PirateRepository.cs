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
    }
}
