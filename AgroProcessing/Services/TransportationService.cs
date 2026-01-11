using AgroProcessing.Data;
using AgroProcessing.Domain.Entries;
using AgroProcessing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgroProcessing.Services
{
    public class TransportationService : ITransportationService
    {
        private readonly AppDbContext _context;

        public TransportationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transportation> RecordTransportationAsync(string relatedType, Guid relatedId, Guid fromLocationId, Guid toLocationId, decimal cost)
        {
            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative");

            if (fromLocationId == toLocationId)
                throw new ArgumentException("From and To locations cannot be the same");

            var fromLocation = await _context.Locations.FindAsync(fromLocationId);
            if (fromLocation == null)
                throw new InvalidOperationException("From location not found");

            var toLocation = await _context.Locations.FindAsync(toLocationId);
            if (toLocation == null)
                throw new InvalidOperationException("To location not found");

            var transportation = new Transportation
            {
                TransportationId = Guid.NewGuid(),
                RelatedType = relatedType,
                RelatedId = relatedId,
                FromLocationId = fromLocationId,
                ToLocationId = toLocationId,
                Cost = cost,
                PaymentStatus = "Unpaid"
            };

            _context.Transportations.Add(transportation);
            await _context.SaveChangesAsync();

            return transportation;
        }

        public async Task<IEnumerable<Transportation>> GetTransportationByTypeAsync(string relatedType, Guid relatedId)
        {
            return await _context.Transportations
                .Where(t => t.RelatedType == relatedType && t.RelatedId == relatedId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalTransportationCostAsync(string relatedType, Guid relatedId)
        {
            return await _context.Transportations
                .Where(t => t.RelatedType == relatedType && t.RelatedId == relatedId)
                .SumAsync(t => t.Cost);
        }

        public async Task<bool> MarkTransportationAsPaidAsync(Guid transportationId)
        {
            var transportation = await _context.Transportations.FindAsync(transportationId);
            if (transportation == null)
                return false;

            transportation.PaymentStatus = "Paid";
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Transportation>> GetUnpaidTransportationsAsync()
        {
            return await _context.Transportations
                .Where(t => t.PaymentStatus == "Unpaid")
                .OrderByDescending(t => t.Cost)
                .ToListAsync();
        }
    }
}
