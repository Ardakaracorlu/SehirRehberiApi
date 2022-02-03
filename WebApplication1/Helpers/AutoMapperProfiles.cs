using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            // City Tablosu için Dto muzun içinden photos tablosundan Url Bağlantısını alıp Map ediyor Controllerde bunu çağıracağız.
            CreateMap<City, CityForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url); 
                });


            CreateMap<City, CityForDetailDto>();

            CreateMap<Photo, PhotoForCreatinDto>();

            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForReturnDto, Photo>();
        }
    }
}
