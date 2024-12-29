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

        public GuestService(IGenericRepository<Guest> genericRepository,IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }
        public async Task AddGuestAsync(GuestDto guestDto)
        {
            await _genericRepository.AddAsync(_mapper.Map<Guest>(guestDto));
        }

        public async Task DeleteGuestAsync(int Id)
        {
            await _genericRepository.DeleteAsync(Id);
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
            await _genericRepository.UpdateAsync(_mapper.Map<Guest>(guestDto), Id);
        }
    }
}
