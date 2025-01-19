using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class RoleService : IRolesService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly ILogger<RoleService> _logger;

        public RoleService(WebCinemaDBContext dbContext, ILogger<RoleService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<RoleDto> GetRoleByIdAsync(int id)
        {
            try
            {
                var role = await _dbContext.Roles
                    .Where(r => r.Id == id)
                    .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .FirstOrDefaultAsync();

                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving role with ID {id}");
                return null;
            }
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _dbContext.Roles
                    .Select(r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name
                    })
                    .ToListAsync();

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all roles");
                return new List<RoleDto>();
            }
        }
    }
}