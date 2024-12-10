using Microsoft.EntityFrameworkCore;
using WebCinema.Interfaces;
using WebCinema.Models;

namespace WebCinema.Services
{
    
    public class PaymentsService : IPaymentsService
    {

        private readonly WebCinemaDBContext _dbContext;
        public PaymentsService(WebCinemaDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Payments> CreatePaymentsAsync(Payments payments)
        {
            if (payments == null)
            {
                return null;
            }
            await _dbContext.Payments.AddAsync(payments);
            await _dbContext.SaveChangesAsync();
            return payments;
        }

        public async Task<Payments> DeletePaymentsByIdAsync(int id)
        {
            var payments = await GetPaymentsByIdAsync(id);
            if (payments != null)
            {
                _dbContext.Payments.Remove(payments);
                await _dbContext.SaveChangesAsync();
            }
            return payments;
        }

        public async Task<List<Payments>> GetAllPaymentsAsync()
        {
            var payments = await _dbContext.Payments.ToListAsync();
            return payments;
        }

        public async Task<Payments> GetPaymentsByIdAsync(int id)
        {
            var payments = await _dbContext.Payments.FirstOrDefaultAsync(x => x.Id == id);
            return payments;
        }

        public async Task<Payments> UpdatePaymentsAsync(int id, Payments payments)
        {
            var _payments = await GetPaymentsByIdAsync(id);
            if (payments != null)
            {
                
                _payments.PaymentMethod= payments.PaymentMethod;
                _payments.TransactionID = payments.TransactionID;
                _payments.PaymentAmount=payments.PaymentAmount;
                _payments.PaymentStatus=payments.PaymentStatus;
                _payments.PaymentDateTime=payments.PaymentDateTime;

                _dbContext.Payments.Update(_payments);
                await _dbContext.SaveChangesAsync();
            }
            return _payments;
        }
    }
}
