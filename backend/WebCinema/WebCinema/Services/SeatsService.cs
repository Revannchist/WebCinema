using Microsoft.EntityFrameworkCore;
using System;
using WebCinema.Interfaces;
using WebCinema.Models;

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
            var seats = await GetSeatsByIdAsync(id);
            if (seats != null)
            {
                _dbContext.Seats.Remove(seats);
                await _dbContext.SaveChangesAsync();
            }
            return seats;
        }

        public async Task<List<Seats>> GetAllSeatsAsync()
        {
            var seats = await _dbContext.Seats.ToListAsync();
            return seats;
        }

        public async Task<Seats> GetSeatsByIdAsync(int id)
        {
            var seats = await _dbContext.Seats.FirstOrDefaultAsync(x => x.Id == id);
            seats.Hall = await _hallsService.GetHallsByIdAsync(seats.HallsId);
            return seats;
        }

        public async Task<Seats> UpdateSeatsAsync(int id, Seats seats)
        {
            var _seats = await GetSeatsByIdAsync(id);
            if (seats != null)
            {
                _seats.SeatNumber = seats.SeatNumber;
                _seats.SeatNumber=seats.SeatNumber;
                _dbContext.Seats.Update(_seats);
                await _dbContext.SaveChangesAsync();
            }
            return _seats;
        }
    }
}
