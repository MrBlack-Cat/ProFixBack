using Repository.Common;


public interface IServiceBookingRepository : IRepository<ServiceBooking>
{
    Task<List<ServiceBooking>> GetByClientProfileIdAsync(int clientProfileId);
    Task<List<ServiceBooking>> GetByProviderProfileIdAsync(int providerProfileId);
    Task<ServiceBooking?> GetDetailedByIdAsync(int id);
    Task<IEnumerable<ServiceBooking>> GetAllAsync();
    Task<bool> IsTimeSlotAvailableAsync(int providerId, DateTime date, TimeSpan start, TimeSpan end);
    Task<List<ServiceBooking>> GetByDateAsync(int providerId, DateTime date);

}
