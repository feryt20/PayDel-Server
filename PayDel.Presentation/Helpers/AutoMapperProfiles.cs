﻿using AutoMapper;
using PayDel.Common.Helpers;
using PayDel.Data.Dtos.Site.Admin;
using PayDel.Data.Dtos.Site.Users;
using PayDel.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayDel.Presentation.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDetailedDto>();
            //CreateMap<User, UserProfileDto>();
            CreateMap<User, UserProfileDto>()
                .ForMember(dest=>dest.ImageUrl, opt=>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                }).ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateOfBirth.ToAge());
                });

            CreateMap<Photo, UserPhotoDto>();
            CreateMap<BankCard, UserBankCardDto>();
            CreateMap<UserForUpdateDto, User>();

            CreateMap<User, UserForLoginDto> ();

            CreateMap<Wallet, WalletForReturnDto>();
            CreateMap<List<WalletForReturnDto>, List<Wallet>>();

            CreateMap<GateForCreateDto, Gate>();
            CreateMap<List<GateForReturnDto>, List<Gate>>();
            CreateMap<Gate, GateForReturnDto>();
        }
    }
}
