using BLL.AllDto;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractService
{
    public interface IPostService
    {
        Task AddPostAsync(PostDto postDto);
        Task UpdatepostAsync(PostDto postDto, int Id);
        Task DeletePostAsync(int Id);
        Task<List<PostDto>> GetAllPostAsync();
        Task<List<Post>> GetPostsByGuestIdAsync(int guestId);
        Task<PostDto> GetPostByIdAsync(int Id);
       
        

    }
}
