using System.Collections.Generic;
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
            return _context.Requests;
        }

        public async Task AddRequest(Request newRequest)
        {
            await _context.Requests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<Request> GetRequestById(int id)
        {
            return await _context.Requests.SingleOrDefaultAsync(request => request.Id == id);
        }

        public IEnumerable<Request> GetOpenRequests()
        {

            return _context.Requests.Where(g => g.IsOpen);

        }


        public async Task UpdateIsOpen(Request request)
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDate(Request request)
        {
            await _context.SaveChangesAsync();
        }

    }
}