using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PirateLibrary.API.Migrations
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Entities.Author, Models.AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    sou => sou.MapFrom(src => $"{src.FirstName} {src.LastName}")
                )
                .ForMember(
                    dest => dest.Age,
                    sou => sou.MapFrom(src => src.DateOfBirth.GetAge())
                );
            CreateMap<Models.AuthorForCreationDto, Entities.Author>();
            CreateMap<Models.AuthorForUpdateDto, Entities.Author>();
            CreateMap<Entities.Author, Models.AuthorForUpdateDto>();
        }
    }
}
