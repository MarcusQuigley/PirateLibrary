using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PirateLibrary.API
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetAge(this DateTimeOffset dateOfBirth)
        {
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateOfBirth.Year;
            if (dateOfBirth.AddYears(age) > currentDate)
            {
                age--;
            }
            return age;
        }
    }
}
