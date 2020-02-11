using System;
using Xunit;

namespace PirateLibrary.API.Tests
{
    public class DateTimeOffsetExtensionsShould
    {
        [Theory]
        [InlineData("10,22,73",46)]
        [InlineData("1,2,73", 47)]
        [InlineData("1,14,73", 47)]
        [InlineData("1,15,73", 46)]
        public void ReturnCorrectAges(string dob, int expectedAge)
        {
            DateTimeOffset parsedDob = DateTimeOffset.Parse(dob); 
            int age = parsedDob.GetAge();
           
            Assert.Equal(expectedAge, age);


        }
    }
}
