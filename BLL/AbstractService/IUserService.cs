using BLL.AllDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractService
{
    public interface IUserService
    {
        Task AddUserAsync(UserDto userDto );
        Task UpdateUserAsync(UserDto userDto, int Id);
        Task DeleteUserAsync(int Id);
        Task<List<UserDto>> GetAllUserAsync();
    }
}
