using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PirateLibrary.API.Models
{
    public class AuthorForUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; }
        public string Title { get; set; }

        //public ICollection<CourseForCreationDto> Courses { get; set; }
        //= new List<CourseForCreationDto>();
    }
}
