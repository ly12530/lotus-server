using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;

namespace Infrastructure
{
    public class RequestDateRepository : IRequestDateRepository
    {
        private readonly LotusDbContext _context;

        public RequestDateRepository(LotusDbContext context)
        {
            _context = context;
        }

        public IQueryable<RequestDate> GetAllRequestDates()
        {
            return _context.RequestDates;
        }

        public async Task AddRequestDate(RequestDate newRequestDate)
        {
            await _context.RequestDates.AddAsync(newRequestDate);
            await _context.SaveChangesAsync();
        }
    }
}