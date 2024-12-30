using AutoMapper;
using BLL.AllDto;
using BlogMvc.ViewModel;
using DAL.Entities;

namespace BlogMvc.MapperProfile
{
    public class MappersProfile : Profile
    {
        public MappersProfile()
        {
            CreateMap<GuestDto, GuestViewModel>().ReverseMap();

            // PostDto <-> PostViewModel
            CreateMap<PostDto, PostViewModel>().ReverseMap();

            // Post -> PostViewModel
            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Guests, opt => opt.MapFrom(src => src.Guests)) // Guest bilgisini eşle
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Guests.Name)); // Yazarın adını eşle

            // Guest -> GuestViewModel
            CreateMap<Guest, GuestViewModel>().ReverseMap();
        }
    }
    }
