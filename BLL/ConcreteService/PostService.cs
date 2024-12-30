using AutoMapper;
using BLL.AbstractService;
using BLL.AllDto;
using DAL.AbstractRepository;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteService
{
    public class PostService : IPostService
    {
        private readonly IGenericRepository<Post> _genericRepository;
        private readonly IMapper _mapper;

        public PostService(IGenericRepository<Post> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        public async Task AddPostAsync(PostDto postDto)
        {
            await _genericRepository.AddAsync(_mapper.Map<Post>(postDto));
        }

        public async Task DeletePostAsync(int Id)
        {
            var post = await _genericRepository.GetByIdAsync(Id);
            if (post != null)
            {
                await _genericRepository.DeleteAsync(Id);
            }
        }

        public async Task<List<PostDto>> GetAllPostAsync()
        {
            return _mapper.Map<List<PostDto>>(await _genericRepository.GetAllAsync());
        }

        public async Task UpdatepostAsync(PostDto postDto, int Id)
        {
            await _genericRepository.UpdateAsync(_mapper.Map<Post>(postDto), Id);
        }
        public async Task<List<Post>> GetPostsByGuestIdAsync(int guestId)
        {
            var posts = await _genericRepository.GetAllAsync();
            return posts.Where(p => p.GuestId == guestId).ToList();
        }

        public async Task<PostDto> GetPostByIdAsync(int Id)
        {
            var post = await _genericRepository.GetByIdAsync(Id);
            if (post == null)
            {
                return null; 
            }

            return _mapper.Map<PostDto>(post);

        }

       

    }
}

