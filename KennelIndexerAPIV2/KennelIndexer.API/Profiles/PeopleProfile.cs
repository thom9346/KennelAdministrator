using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Profiles
{
    public class PeopleProfile : Profile
    {
        public PeopleProfile()
        {
            CreateMap<Entities.Person, Models.PersonDto>();
            CreateMap<Models.PersonForCreationDto, Entities.Person>();
            CreateMap<Models.PersonDto, Entities.Person>();
        }
    }
}
