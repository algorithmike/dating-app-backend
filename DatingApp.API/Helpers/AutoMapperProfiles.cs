using System;
using System.Linq;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDTO>()
                .ForMember(
                    destination => destination.PhotoURL,
                    option => option.MapFrom(source =>
                        source.Photos.FirstOrDefault(photo => photo.IsMain).Url)
                ).ForMember(
                    destination => destination.Age,
                    option => option.MapFrom(source => source.DateOfBirth.CalculateAge())
                );
            CreateMap<User, UserForDetailedDTO>()
                .ForMember(
                    destination => destination.PhotoURL,
                    option => option.MapFrom(source =>
                        source.Photos.FirstOrDefault(photo => photo.IsMain).Url)
                ).ForMember(
                    destination => destination.Age,
                    option => option.MapFrom(source => source.DateOfBirth.CalculateAge())
                );
            CreateMap<Photo, PhotosForDetailedDTO>();
        }
    }
}
