using System;
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
            return _context.Requests
                .Include(req => req.Customer)
                .Include(req => req.DesignatedUser)
                .Include(req => req.Subscribers);
        }

        public async Task AddRequest(Request newRequest)
        {
            await _context.Requests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<Request> GetRequestById(int id)
        {
            return await _context.Requests
                .Include(req => req.Subscribers)
                .Include(req => req.DesignatedUser)
                .FirstOrDefaultAsync(req => req.Id == id);
        }

        public IEnumerable<Request> GetOpenRequests()
        {
            return _context.Requests.Where(g => g.IsOpen);
        }

        public async Task UpdateRequest(Request request)
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRequest(Request request)
        {

            if (request == null) throw new ArgumentNullException(nameof(request));

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

        }
       

    }
}