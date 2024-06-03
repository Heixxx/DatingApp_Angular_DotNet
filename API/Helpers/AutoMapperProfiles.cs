using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles: Profile{
    public AutoMapperProfiles(){                 //      Z appuser do MemberDTO
        CreateMap<AppUser, MemberDto>()      //Łączy konkretną wartość (photoUrl) z konkretną wartością 
            .ForMember(dest => dest.PhotoUrl, opt =>  opt.MapFrom(src => src.Photos.FirstOrDefault(x=> x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();
    }
}