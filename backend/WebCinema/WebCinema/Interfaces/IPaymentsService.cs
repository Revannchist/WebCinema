using WebCinema.Models;

namespace WebCinema.Interfaces
{
    public interface IPaymentsService
    {
        Task<Payments> CreatePaymentsAsync(Payments payments);

        Task<List<Payments>> GetAllPaymentsAsync();

        Task<Payments> GetPaymentsByIdAsync(int id);

        Task<Payments> DeletePaymentsByIdAsync(int id);

        Task<Payments> UpdatePaymentsAsync(int id, Payments payments);
    }
}
