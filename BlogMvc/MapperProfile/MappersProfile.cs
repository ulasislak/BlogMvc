using AutoMapper;
using BLL.AllDto;
using BlogMvc.ViewModel;

namespace BlogMvc.MapperProfile
{
    public class MappersProfile : Profile
    {
        public MappersProfile()
        {
            // GuestDto <-> GuestViewModel için mapleme
            CreateMap<GuestDto, GuestViewModel>().ReverseMap();

            // PostDto <-> PostViewModel için mapleme
            CreateMap<PostDto, PostViewModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // PostDto -> PostViewModel için Id alanını ihmal et
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // PostViewModel -> PostDto için Id alanını ihmal et
        }
    }
}