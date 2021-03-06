using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices
{
    public interface IRequestRepository
    {
        IQueryable<Request> GetAllRequests();
        Task AddRequest(Request newRequest);
        Task<Request> GetRequestById(int id);
        Task UpdateRequest(Request request);   
        IEnumerable<Request> GetOpenRequests();
        Task DeleteRequest(Request request);
    }
}
