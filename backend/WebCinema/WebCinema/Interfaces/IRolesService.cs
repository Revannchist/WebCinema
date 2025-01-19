using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IRolesService
    {
        Task<RoleDto> GetRoleByIdAsync(int id);
        Task<List<RoleDto>> GetAllRolesAsync();
    }
}
