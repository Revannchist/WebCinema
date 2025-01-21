using Microsoft.EntityFrameworkCore;
using System;
using WebCinema.Interfaces;
using WebCinema.Models;
using WebCinema.Models.DTO;

namespace WebCinema.Services
{
    public class SeatsService : ISeatsService
    {
        private readonly WebCinemaDBContext _dbContext;
        private readonly IHallsService _hallsService;
        public SeatsService(WebCinemaDBContext dbContext,IHallsService hallsService)
        {
            _dbContext = dbContext;
            _hallsService = hallsService;
        }
        public async Task<Seats> CreateSeatsAsync(Seats seats)
        {
            if (seats == null)
            {
                return null;
            }
            await _dbContext.Seats.AddAsync(seats);
            await _dbContext.SaveChangesAsync();
            return seats;
        }

        public async Task<Seats> DeleteSeatsByIdAsync(int id)
        {
            var seats = await _dbContext.Seats.FirstOrDefaultAsync(x => x.Id == id);
            if (seats != null)
            {
                _dbContext.Seats.Remove(seats);
                await _dbContext.SaveChangesAsync();
            }
            return seats;
        }

        public async Task<List<SeatsDto>> GetAllSeatsAsync()
        {
            var seats = await _dbContext.Seats
                .Include(s => s.Hall)
                .Select(s => new SeatsDto
                {
                    Id = s.Id,
                    HallsId = s.HallsId,
                    HallName = s.Hall.HallName,
                    SeatNumber = s.SeatNumber,
                    SeatType = s.SeatType
                })
                .ToListAsync();

            return seats;
        }

        public async Task<SeatsDto> GetSeatsByIdAsync(int id)
        {
            var seat = await _dbContext.Seats
                .Include(s => s.Hall)
                .Where(x => x.Id == id)
                .Select(s => new SeatsDto
                {
                    Id = s.Id,
                    HallsId = s.HallsId,
                    HallName = s.Hall.HallName,
                    SeatNumber = s.SeatNumber,
                    SeatType = s.SeatType
                })
                .FirstOrDefaultAsync();

            return seat;
        }

        public async Task<Seats> UpdateSeatsAsync(int id, Seats seats)
        {
            var _seats = await _dbContext.Seats.FirstOrDefaultAsync(x => x.Id == id);
            if (_seats != null)
            {
                _seats.SeatNumber = seats.SeatNumber;
                _seats.SeatType = seats.SeatType;
                _dbContext.Seats.Update(_seats);
                await _dbContext.SaveChangesAsync();
            }
            return _seats;
        }
    }
}
