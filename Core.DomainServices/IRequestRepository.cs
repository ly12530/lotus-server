using System.Linq;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices
{
    public interface IRequestRepository
    {
        IQueryable<Request> GetAllRequests();
        Task AddRequest(Request newRequest);
    }
}