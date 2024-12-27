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
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _genericRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> genericRepository,IMapper mapper)
        {
           _genericRepository = genericRepository;
           _mapper = mapper;
        }
        public async Task AddUserAsync(UserDto userDto)
        {
            await _genericRepository.AddAsync(_mapper.Map<User>(userDto));
        }

        public async Task DeleteUserAsync(int Id)
        {
            await _genericRepository.DeleteAsync(Id);
        }

        public async Task<List<UserDto>> GetAllUserAsync()
        {
            return  _mapper.Map<List<UserDto>>(await _genericRepository.GetAllAsync());
        }

        public async Task UpdateUserAsync(UserDto userDto, int Id)
        {
            await _genericRepository.UpdateAsync(_mapper.Map<User>(userDto), Id);
        }
    }
}
