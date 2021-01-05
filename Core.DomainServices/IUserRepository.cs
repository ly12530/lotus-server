using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices
{
    public interface IUserRepository
    {
        Task UpdateUser(User user);
        Task<User> GetUserById(int id);
    }
}
