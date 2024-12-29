using AutoMapper;
using BLL.AllDto;
using BLL.ConcreteService;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.MapperProfile
{
    public class MapperProfiles:Profile
    {
        public MapperProfiles()
        {
            CreateMap<GuestDto,Guest>().ReverseMap();
            CreateMap<PostDto, Post>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
