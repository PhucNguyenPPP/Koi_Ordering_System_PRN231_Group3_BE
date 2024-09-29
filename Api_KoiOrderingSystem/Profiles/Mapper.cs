﻿using AutoMapper;
using Common.DTO.Auth;
using Common.DTO.FarmImage;
using Common.DTO.KoiFish;
using Common.DTO.User;
using DAL.Entities;

namespace Api_KoiOrderingSystem.Profiles
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            #region
            CreateMap<SignUpCustomerRequestDTO, User>().ReverseMap();
            CreateMap<User, LocalUserDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ReverseMap();
            CreateMap<Koi, KoiDTO>()
                //.ForMember(dest => dest.FarmId, opt => opt.MapFrom(src => src.Farm.UserId))
                .ReverseMap();
			CreateMap<User, FarmDetailDTO>().ReverseMap();
			CreateMap<FarmImage, FarmImageDTO>().ReverseMap();
			CreateMap<SignUpFarmRequestDTO, User>().ReverseMap();
            CreateMap<GetAllKoiDTO, Koi>().ReverseMap();
			#endregion
		}
    }
}
