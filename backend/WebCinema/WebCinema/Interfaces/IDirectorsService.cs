using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Interfaces
{
    public interface IDirectorsService
    {
        Task<DirectorGetDto> CreateDirectorAsync(DirectorCreateDto directorDto);

        Task<List<DirectorGetDto>> GetAllDirectorsAsync();

        Task<DirectorGetDto> GetDirectorByIdAsync(int id);

        Task<DirectorGetDto> DeleteDirectorByIdAsync(int id);

        Task<DirectorGetDto> UpdateDirectorAsync(int id, DirectorUpdateDto directorDto);
    }
}