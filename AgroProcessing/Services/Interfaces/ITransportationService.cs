using AgroProcessing.Domain.Entries;

namespace AgroProcessing.Services.Interfaces
{
    public interface ITransportationService
    {
        Task<Transportation> RecordTransportationAsync(string relatedType, Guid relatedId, Guid fromLocationId, Guid toLocationId, decimal cost);
        Task<IEnumerable<Transportation>> GetTransportationByTypeAsync(string relatedType, Guid relatedId);
        Task<decimal> GetTotalTransportationCostAsync(string relatedType, Guid relatedId);
        Task<bool> MarkTransportationAsPaidAsync(Guid transportationId);
        Task<IEnumerable<Transportation>> GetUnpaidTransportationsAsync();
    }
}
