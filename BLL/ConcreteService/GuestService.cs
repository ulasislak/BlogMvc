using AutoMapper;
using BLL.AbstractService;
using BLL.AllDto;
using DAL.AbstractRepository;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ConcreteService
{
    public class GuestService : IGuestService
    {
        private readonly IGenericRepository<Guest> _genericRepository;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;

        public GuestService(IGenericRepository<Guest> genericRepository,IMapper mapper, IPostService postService)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _postService = postService;
        }
        public async Task AddGuestAsync(GuestDto guestDto)
        {
            await _genericRepository.AddAsync(_mapper.Map<Guest>(guestDto));
        }

        public async Task<bool> DeleteGuestAsync(int Id)
        {
            var guest = await _genericRepository.GetByIdAsync(Id);
            if (guest == null)
            {
                return false; // Kullanıcı bulunamadı
            }

            var posts = await _postService.GetPostsByGuestIdAsync(Id);
            foreach (var post in posts)
            {
                await _postService.DeletePostAsync(post.Id);
            }

            await _genericRepository.DeleteAsync(Id);
            return true;
        }

        public async Task<List<GuestDto>> GetAllGuestAsync()
        {
            return _mapper.Map<List<GuestDto>>(await _genericRepository.GetAllAsync());
        }

        public async Task<GuestDto> GetGuestByIdAsync(int Id)
        {
            var GetId=await _genericRepository.GetByIdAsync(Id);
            return _mapper.Map<GuestDto>(GetId);
        }

        public async Task UpdateGuestAsync(GuestDto guestDto, int Id)
        {
            var guest = _mapper.Map<Guest>(guestDto);
            await _genericRepository.UpdateAsync(guest, Id);
        }
    }
}
