using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelIndexer.API.Profiles
{
    public class PicturesProfile : Profile
    {
        public PicturesProfile()
        {
            CreateMap<Entities.Picture, Models.PictureDto>();
            CreateMap<Models.PictureForCreationDto, Entities.Picture>();
        }
    }
}
