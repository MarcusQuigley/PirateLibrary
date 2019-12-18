using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PirateLibrary.API.Services
{
    public class PirateRepository
    {
        readonly PirateLibraryContext context;

        public PirateRepository(PirateLibraryContext context)
        {
            this.context = context;
        }
    }
}
