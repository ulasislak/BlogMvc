using BLL.AllDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AbstractService
{
    public interface IGuestService
    {
        Task AddGuestAsync(GuestDto guestDto);
        Task UpdateGuestAsync(GuestDto guestDto, int Id);
        Task<bool> DeleteGuestAsync(int Id);
        Task<List<GuestDto>> GetAllGuestAsync();
        Task<GuestDto> GetGuestByIdAsync(int Id);

    }
}
