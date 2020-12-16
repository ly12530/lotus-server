using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class RequestRepository : IRequestRepository
    {
        private readonly LotusDbContext _context;

        public RequestRepository(LotusDbContext context)
        {
            _context = context;
        }

        public IQueryable<Request> GetAllRequests()
        {
            return _context.Requests
                .Include(request => request.RequestDate);
        }

        public async Task AddRequest(Request newRequest)
        {
            await _context.Requests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
        }
    }
}