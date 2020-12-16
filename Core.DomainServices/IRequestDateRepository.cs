using System.Linq;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices
{
    public interface IRequestDateRepository
    {
        IQueryable<RequestDate> GetAllRequestDates();

        Task AddRequestDate(RequestDate newRequestDate);
    }
}